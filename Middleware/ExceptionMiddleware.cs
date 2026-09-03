using System.Net;
using System.Text.Json;

namespace LiveProject_WebAPI_Demo.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }


    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        context.Response.ContentType = "application/json";


        var response = new
        {
            message = "Something went wrong",
            error = ex.Message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    
}