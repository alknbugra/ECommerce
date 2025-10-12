using ECommerce.API.Common.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Diagnostics;

namespace ECommerce.API.Common.Extensions;

/// <summary>
/// Logging extension metodları
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Serilog yapılandırması ekle
    /// </summary>
    /// <param name="builder">WebApplicationBuilder</param>
    /// <returns>WebApplicationBuilder</returns>
    public static WebApplicationBuilder AddSerilogConfiguration(this WebApplicationBuilder builder)
    {
        var serilogConfig = builder.Configuration.GetSection(SerilogConfiguration.SectionName)
            .Get<SerilogConfiguration>() ?? new SerilogConfiguration();

        // Serilog logger yapılandırması
        var loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Is(Enum.Parse<LogEventLevel>(serilogConfig.MinimumLevel))
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "ECommerce.API")
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .Enrich.WithProperty("MachineName", Environment.MachineName)
            .Enrich.WithProperty("ProcessId", Environment.ProcessId)
            .Enrich.WithProperty("ThreadId", Environment.CurrentManagedThreadId);

        // Console sink
        if (serilogConfig.EnableConsoleSink)
        {
            if (serilogConfig.UseJsonFormat)
            {
                loggerConfig.WriteTo.Console(new JsonFormatter());
            }
            else
            {
                loggerConfig.WriteTo.Console(
                    outputTemplate: serilogConfig.OutputTemplate);
            }
        }

        // File sink
        if (serilogConfig.EnableFileSink)
        {
            var rollingInterval = Enum.Parse<RollingInterval>(serilogConfig.RollingInterval);
            
            if (serilogConfig.UseJsonFormat)
            {
                loggerConfig.WriteTo.File(
                    new JsonFormatter(),
                    serilogConfig.LogFilePath,
                    rollingInterval: rollingInterval,
                    fileSizeLimitBytes: serilogConfig.FileSizeLimitMB * 1024 * 1024,
                    retainedFileCountLimit: serilogConfig.RetainedFileCountLimit);
            }
            else
            {
                loggerConfig.WriteTo.File(
                    serilogConfig.LogFilePath,
                    outputTemplate: serilogConfig.OutputTemplate,
                    rollingInterval: rollingInterval,
                    fileSizeLimitBytes: serilogConfig.FileSizeLimitMB * 1024 * 1024,
                    retainedFileCountLimit: serilogConfig.RetainedFileCountLimit);
            }
        }

        // OpenSearch sink
        loggerConfig.AddOpenSearchSink(builder.Configuration, builder.Environment);

        // Serilog'u global logger olarak ayarla
        Log.Logger = loggerConfig.CreateLogger();
        
        // Host builder'a Serilog'u ekle
        builder.Host.UseSerilog();

        return builder;
    }

    /// <summary>
    /// OpenTelemetry yapılandırması ekle
    /// </summary>
    /// <param name="builder">WebApplicationBuilder</param>
    /// <returns>WebApplicationBuilder</returns>
    public static WebApplicationBuilder AddOpenTelemetryConfiguration(this WebApplicationBuilder builder)
    {
        var otelConfig = builder.Configuration.GetSection(OpenTelemetryConfiguration.SectionName)
            .Get<OpenTelemetryConfiguration>() ?? new OpenTelemetryConfiguration();

        if (!otelConfig.Enabled)
            return builder;

        // OpenTelemetry resource bilgileri
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(
                serviceName: otelConfig.ServiceName,
                serviceVersion: otelConfig.ServiceVersion,
                serviceNamespace: otelConfig.ServiceNamespace)
            .AddAttributes(new Dictionary<string, object>
            {
                ["deployment.environment"] = builder.Environment.EnvironmentName,
                ["host.name"] = Environment.MachineName,
                ["process.pid"] = Environment.ProcessId
            });

        // OpenTelemetry services ekle
        builder.Services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .SetResourceBuilder(resourceBuilder)
                    .SetSampler(new TraceIdRatioBasedSampler(otelConfig.SamplingRatio))
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.EnrichWithHttpRequest = (activity, httpRequest) =>
                        {
                            activity.SetTag("http.request.body.size", httpRequest.ContentLength);
                        };
                        options.EnrichWithHttpResponse = (activity, httpResponse) =>
                        {
                            activity.SetTag("http.response.body.size", httpResponse.ContentLength);
                        };
                    })
                    .AddEntityFrameworkCoreInstrumentation(options =>
                    {
                        options.SetDbStatementForText = true;
                        options.SetDbStatementForStoredProcedure = true;
                    })
                    .AddHttpClientInstrumentation();

                // Console exporter
                if (otelConfig.EnableConsoleExporter)
                {
                    tracerProviderBuilder.AddConsoleExporter();
                }

                // OTLP exporter (Jaeger, Zipkin, etc.)
                if (!string.IsNullOrEmpty(otelConfig.OtlpEndpoint))
                {
                    tracerProviderBuilder.AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(otelConfig.OtlpEndpoint);
                        options.Protocol = OtlpExportProtocol.Grpc;
                    });
                }
            })
            .WithMetrics(metricsProviderBuilder =>
            {
                metricsProviderBuilder
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation();

                // Console exporter
                if (otelConfig.EnableConsoleExporter)
                {
                    metricsProviderBuilder.AddConsoleExporter();
                }

                // OTLP exporter
                if (!string.IsNullOrEmpty(otelConfig.OtlpEndpoint))
                {
                    metricsProviderBuilder.AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(otelConfig.OtlpEndpoint);
                        options.Protocol = OtlpExportProtocol.Grpc;
                    });
                }
            })
            .WithLogging(loggingBuilder =>
            {
                loggingBuilder
                    .SetResourceBuilder(resourceBuilder);

                // Console exporter
                if (otelConfig.EnableConsoleExporter)
                {
                    loggingBuilder.AddConsoleExporter();
                }

                // OTLP exporter
                if (!string.IsNullOrEmpty(otelConfig.OtlpEndpoint))
                {
                    loggingBuilder.AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(otelConfig.OtlpEndpoint);
                        options.Protocol = OtlpExportProtocol.Grpc;
                    });
                }
            });

        return builder;
    }

    /// <summary>
    /// Request logging middleware ekle
    /// </summary>
    /// <param name="app">WebApplication</param>
    /// <returns>WebApplication</returns>
    public static WebApplication UseRequestLogging(this WebApplication app)
    {
        var serilogConfig = app.Configuration.GetSection(SerilogConfiguration.SectionName)
            .Get<SerilogConfiguration>() ?? new SerilogConfiguration();

        if (serilogConfig.EnableRequestLogging)
        {
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
                options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Information;
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                    diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault() ?? "Unknown");
                    diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown");
                    
                    // User bilgileri (eğer authenticated ise)
                    if (httpContext.User.Identity?.IsAuthenticated == true)
                    {
                        diagnosticContext.Set("UserId", httpContext.User.FindFirst("sub")?.Value ?? "Unknown");
                        diagnosticContext.Set("UserName", httpContext.User.FindFirst("name")?.Value ?? "Unknown");
                    }
                };
            });
        }

        return app;
    }

    /// <summary>
    /// Performance logging middleware ekle
    /// </summary>
    /// <param name="app">WebApplication</param>
    /// <returns>WebApplication</returns>
    public static WebApplication UsePerformanceLogging(this WebApplication app)
    {
        var serilogConfig = app.Configuration.GetSection(SerilogConfiguration.SectionName)
            .Get<SerilogConfiguration>() ?? new SerilogConfiguration();

        if (serilogConfig.EnablePerformanceLogging)
        {
            app.Use(async (context, next) =>
            {
                var stopwatch = Stopwatch.StartNew();
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

                try
                {
                    await next();
                }
                finally
                {
                    stopwatch.Stop();
                    
                    if (stopwatch.ElapsedMilliseconds > 1000) // 1 saniyeden uzun süren istekler
                    {
                        logger.LogWarning("Slow request detected: {Method} {Path} took {ElapsedMs}ms",
                            context.Request.Method,
                            context.Request.Path,
                            stopwatch.ElapsedMilliseconds);
                    }
                }
            });
        }

        return app;
    }
}
