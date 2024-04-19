using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FurnStore.Middleware;

// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class NotFound
{
    private readonly RequestDelegate _next;

    public NotFound(RequestDelegate next)
    {
        _next = next;
    }

    public async Task<Task> Invoke(HttpContext httpContext)
    {
        if (httpContext.Response.StatusCode == 404)
        {
            await httpContext.Response.WriteAsync("Page not found");
        }

        return _next(httpContext);
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class NotFoundExtensions
{
    public static IApplicationBuilder UseNotFound(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<NotFound>();
    }
}