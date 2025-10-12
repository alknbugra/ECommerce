using ECommerce.API.Common.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.OpenSearch;
using Serilog.Sinks.OpenSearch;

namespace ECommerce.API.Common.Extensions;

/// <summary>
/// OpenSearch extension metodları
/// </summary>
public static class OpenSearchExtensions
{
    /// <summary>
    /// OpenSearch sink'i Serilog'a ekle
    /// </summary>
    /// <param name="loggerConfig">Serilog logger configuration</param>
    /// <param name="configuration">IConfiguration</param>
    /// <param name="environment">IHostEnvironment</param>
    /// <returns>LoggerConfiguration</returns>
    public static LoggerConfiguration AddOpenSearchSink(
        this LoggerConfiguration loggerConfig,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var openSearchConfig = configuration.GetSection(OpenSearchConfiguration.SectionName)
            .Get<OpenSearchConfiguration>() ?? new OpenSearchConfiguration();

        if (!openSearchConfig.Enabled)
            return loggerConfig;

        try
        {
            var openSearchSinkOptions = new OpenSearchSinkOptions(new Uri(openSearchConfig.NodeUris.First()))
            {
                IndexFormat = openSearchConfig.IndexFormat,
                AutoRegisterTemplate = openSearchConfig.AutoRegisterTemplate,
                TemplateName = openSearchConfig.IndexTemplateName,
                NumberOfShards = openSearchConfig.NumberOfShards,
                NumberOfReplicas = openSearchConfig.NumberOfReplicas,
                MinimumLogEventLevel = LogEventLevel.Information,
                EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,
                FailureCallback = e => Console.WriteLine($"OpenSearch sink failure: {e}"),
                DeadLetterIndexName = "ecommerce-logs-deadletter",
                CustomFormatter = new OpenSearchJsonFormatter(),
                TypeName = null, // Disable type field
                RegisterTemplateFailure = RegisterTemplateRecovery.IndexAnyway,
                InlineFields = true,
                DetectOpenSearchVersion = true,
                OverwriteTemplate = true
            };

            // Authentication
            if (!string.IsNullOrEmpty(openSearchConfig.Username) && !string.IsNullOrEmpty(openSearchConfig.Password))
            {
                openSearchSinkOptions.ModifyConnectionSettings = (connectionSettings) =>
                {
                    return connectionSettings
                        .BasicAuthentication(openSearchConfig.Username, openSearchConfig.Password);
                };
            }
            else if (!string.IsNullOrEmpty(openSearchConfig.ApiKey))
            {
                openSearchSinkOptions.ModifyConnectionSettings = (connectionSettings) =>
                {
                    return connectionSettings
                        .ApiKeyAuthentication(new OpenSearch.Net.ApiKeyAuthenticationCredentials(openSearchConfig.ApiKey));
                };
            }

            // SSL Configuration
            if (!openSearchConfig.VerifySsl)
            {
                openSearchSinkOptions.ModifyConnectionSettings = (connectionSettings) =>
                {
                    return connectionSettings
                        .ServerCertificateValidationCallback((o, certificate, chain, errors) => true);
                };
            }

            // Certificate Fingerprint
            if (!string.IsNullOrEmpty(openSearchConfig.CertificateFingerprint))
            {
                openSearchSinkOptions.ModifyConnectionSettings = (connectionSettings) =>
                {
                    return connectionSettings
                        .ServerCertificateValidationCallback((o, certificate, chain, errors) =>
                        {
                            if (certificate != null)
                            {
                                var fingerprint = certificate.GetCertHashString();
                                return fingerprint.Equals(openSearchConfig.CertificateFingerprint, StringComparison.OrdinalIgnoreCase);
                            }
                            return false;
                        });
                };
            }

            loggerConfig.WriteTo.OpenSearch(openSearchSinkOptions);

            Console.WriteLine($"OpenSearch sink configured successfully. Nodes: {string.Join(", ", openSearchConfig.NodeUris)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to configure OpenSearch sink: {ex.Message}");
            // Fallback to console logging if OpenSearch fails
            loggerConfig.WriteTo.Console();
        }

        return loggerConfig;
    }

    /// <summary>
    /// OpenSearch health check ekle
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">IConfiguration</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddOpenSearchHealthCheck(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var openSearchConfig = configuration.GetSection(OpenSearchConfiguration.SectionName)
            .Get<OpenSearchConfiguration>() ?? new OpenSearchConfiguration();

        if (!openSearchConfig.Enabled)
            return services;

        services.AddHealthChecks()
            .AddElasticsearch(
                openSearchConfig.NodeUris.First(),
                name: "opensearch",
                failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                tags: new[] { "opensearch", "logging", "external" });

        return services;
    }
}
