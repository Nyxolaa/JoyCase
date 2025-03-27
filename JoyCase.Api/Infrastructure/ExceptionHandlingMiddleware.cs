using System.Net;
using System.Text.Json;
using JoyCase.Api.Log;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly LogService _logService;

    public ExceptionHandlingMiddleware(RequestDelegate next, LogService logService)
    {
        _next = next;
        _logService = logService;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new
        {
            Message = "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.",
            StatusCode = (int)HttpStatusCode.InternalServerError
        };

        // Hata loglama
        _logService.LogError(exception.Message, exception);

        response.StatusCode = errorResponse.StatusCode;
        var result = JsonSerializer.Serialize(errorResponse);
        await response.WriteAsync(result);
    }
}
