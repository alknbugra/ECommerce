using ECommerce.API.Common.ProblemDetails;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace ECommerce.API.Common.Middleware;

/// <summary>
/// Global exception handling middleware
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = CreateErrorResponse(exception, context);

        response.StatusCode = errorResponse.Status;

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        await response.WriteAsync(jsonResponse);
    }

    private static ErrorResponse CreateErrorResponse(Exception exception, HttpContext context)
    {
        return exception switch
        {
            ECommerce.Application.Common.Exceptions.ValidationException validationEx => CreateValidationErrorResponse(validationEx, context),
            NotFoundException notFoundEx => CreateNotFoundErrorResponse(notFoundEx, context),
            BadRequestException badRequestEx => CreateBadRequestErrorResponse(badRequestEx, context),
            UnauthorizedException unauthorizedEx => CreateUnauthorizedErrorResponse(unauthorizedEx, context),
            ForbiddenException forbiddenEx => CreateForbiddenErrorResponse(forbiddenEx, context),
            ECommerce.Application.Common.Exceptions.ApplicationException appEx => CreateApplicationErrorResponse(appEx, context),
            DomainException domainEx => CreateDomainErrorResponse(domainEx, context),
            ArgumentNullException argNullEx => CreateArgumentNullErrorResponse(argNullEx, context),
            ArgumentException argEx => CreateArgumentErrorResponse(argEx, context),
            TimeoutException timeoutEx => CreateTimeoutErrorResponse(timeoutEx, context),
            UnauthorizedAccessException unauthorizedAccessEx => CreateUnauthorizedAccessErrorResponse(unauthorizedAccessEx, context),
            _ => CreateInternalServerErrorResponse(exception, context)
        };
    }

    private static ErrorResponse CreateValidationErrorResponse(ECommerce.Application.Common.Exceptions.ValidationException exception, HttpContext context)
    {
        return new ErrorResponse
        {
            ErrorCode = exception.ErrorCode,
            Message = exception.Message,
            Details = exception.Details,
            ValidationErrors = exception.Errors.Select(e => new ValidationErrorDetail
            {
                PropertyName = e.PropertyName,
                ErrorMessage = e.ErrorMessage,
                AttemptedValue = e.AttemptedValue
            }).ToList(),
            Status = (int)HttpStatusCode.BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Validation Error",
            Instance = context.Request.Path,
            RequestId = context.TraceIdentifier
        };
    }

    private static ErrorResponse CreateNotFoundErrorResponse(NotFoundException exception, HttpContext context)
    {
        return new ErrorResponse
        {
            ErrorCode = exception.ErrorCode,
            Message = exception.Message,
            Details = exception.Details,
            Status = (int)HttpStatusCode.NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "Not Found",
            Instance = context.Request.Path,
            RequestId = context.TraceIdentifier
        };
    }

    private static ErrorResponse CreateBadRequestErrorResponse(BadRequestException exception, HttpContext context)
    {
        return new ErrorResponse
        {
            ErrorCode = exception.ErrorCode,
            Message = exception.Message,
            Details = exception.Details,
            Status = (int)HttpStatusCode.BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Bad Request",
            Instance = context.Request.Path,
            RequestId = context.TraceIdentifier
        };
    }

    private static ErrorResponse CreateUnauthorizedErrorResponse(UnauthorizedException exception, HttpContext context)
    {
        return new ErrorResponse
        {
            ErrorCode = exception.ErrorCode,
            Message = exception.Message,
            Details = exception.Details,
            Status = (int)HttpStatusCode.Unauthorized,
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            Title = "Unauthorized",
            Instance = context.Request.Path,
            RequestId = context.TraceIdentifier
        };
    }

    private static ErrorResponse CreateForbiddenErrorResponse(ForbiddenException exception, HttpContext context)
    {
        return new ErrorResponse
        {
            ErrorCode = exception.ErrorCode,
            Message = exception.Message,
            Details = exception.Details,
            Status = (int)HttpStatusCode.Forbidden,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            Title = "Forbidden",
            Instance = context.Request.Path,
            RequestId = context.TraceIdentifier
        };
    }

    private static ErrorResponse CreateApplicationErrorResponse(ECommerce.Application.Common.Exceptions.ApplicationException exception, HttpContext context)
    {
        return new ErrorResponse
        {
            ErrorCode = exception.ErrorCode,
            Message = exception.Message,
            Details = exception.Details,
            Status = (int)HttpStatusCode.BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Application Error",
            Instance = context.Request.Path,
            RequestId = context.TraceIdentifier
        };
    }

    private static ErrorResponse CreateDomainErrorResponse(DomainException exception, HttpContext context)
    {
        return new ErrorResponse
        {
            ErrorCode = exception.ErrorCode,
            Message = exception.Message,
            Status = (int)HttpStatusCode.BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Domain Error",
            Instance = context.Request.Path,
            RequestId = context.TraceIdentifier
        };
    }


    private static ErrorResponse CreateArgumentErrorResponse(ArgumentException exception, HttpContext context)
    {
        return new ErrorResponse
        {
            ErrorCode = "INVALID_ARGUMENT",
            Message = exception.Message,
            Status = (int)HttpStatusCode.BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Invalid Argument",
            Instance = context.Request.Path,
            RequestId = context.TraceIdentifier
        };
    }

    private static ErrorResponse CreateArgumentNullErrorResponse(ArgumentNullException exception, HttpContext context)
    {
        return new ErrorResponse
        {
            ErrorCode = "NULL_ARGUMENT",
            Message = exception.Message,
            Status = (int)HttpStatusCode.BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Null Argument",
            Instance = context.Request.Path,
            RequestId = context.TraceIdentifier
        };
    }

    private static ErrorResponse CreateTimeoutErrorResponse(TimeoutException exception, HttpContext context)
    {
        return new ErrorResponse
        {
            ErrorCode = "TIMEOUT",
            Message = "The operation timed out.",
            Status = (int)HttpStatusCode.RequestTimeout,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.7",
            Title = "Request Timeout",
            Instance = context.Request.Path,
            RequestId = context.TraceIdentifier
        };
    }

    private static ErrorResponse CreateUnauthorizedAccessErrorResponse(UnauthorizedAccessException exception, HttpContext context)
    {
        return new ErrorResponse
        {
            ErrorCode = "UNAUTHORIZED_ACCESS",
            Message = exception.Message,
            Status = (int)HttpStatusCode.Unauthorized,
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            Title = "Unauthorized Access",
            Instance = context.Request.Path,
            RequestId = context.TraceIdentifier
        };
    }

    private static ErrorResponse CreateInternalServerErrorResponse(Exception exception, HttpContext context)
    {
        return new ErrorResponse
        {
            ErrorCode = "INTERNAL_SERVER_ERROR",
            Message = "An internal server error occurred.",
            Status = (int)HttpStatusCode.InternalServerError,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "Internal Server Error",
            Instance = context.Request.Path,
            RequestId = context.TraceIdentifier
        };
    }
}
