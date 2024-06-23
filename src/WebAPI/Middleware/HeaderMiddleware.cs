using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class HeaderMiddleware
{
    private readonly RequestDelegate _next;

    public HeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Add("X-Custom-Header", "My Custom Value");

        await _next(context);
    }
}