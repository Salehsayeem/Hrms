using HrmsBe.Dto.V1.Common;
using HrmsBe.Helper;
using HrmsBe.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly MongoDbService _mongoDbService;

    public ErrorHandlingMiddleware(RequestDelegate next, MongoDbService mongoDbService)
    {
        _next = next;
        _mongoDbService = mongoDbService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (CustomExceptionWithAudit ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
        }
        catch (Exception ex) // Handle other types of exceptions
        {
            var auditInfo = new AuditDto
            {
                Description = ex.Message,
                ControllerName = context.GetEndpoint()?.DisplayName ?? "Unknown",
                RequestParameters = await GetRequestBodyAsync(context.Request),
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            await HandleExceptionAsync(context, new CustomExceptionWithAudit(ex.Message, auditInfo), HttpStatusCode.InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, CustomExceptionWithAudit exception, HttpStatusCode statusCode)
    {
        var auditInfo = exception.AuditInfo ?? new AuditDto
        {
            Description = exception.Message,
            ControllerName = context.GetEndpoint()?.DisplayName ?? "Unknown",
            RequestParameters = await GetRequestBodyAsync(context.Request),
            StatusCode = (int)statusCode
        };

        await _mongoDbService.CreateOrUpdateAudit(auditInfo);

        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)statusCode;

        var errorResponse = new CommonResponseDto
        {
            Message = exception.Message,
            Data = null, // You can include more data if needed
            Succeed = false,
            StatusCode = (int)statusCode
        };

        var result = JsonSerializer.Serialize(errorResponse);
        await response.WriteAsync(result);
    }

    private async Task<string> GetRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();
        request.Body.Position = 0;

        using var reader = new StreamReader(request.Body);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        return body;
    }
}

public class CustomExceptionWithAudit : Exception
{
    public AuditDto AuditInfo { get; }

    public CustomExceptionWithAudit(string message, AuditDto auditInfo)
        : base(message)
    {
        AuditInfo = auditInfo;
    }
}
