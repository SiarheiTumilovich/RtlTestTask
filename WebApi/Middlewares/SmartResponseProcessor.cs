using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebApi.Middlewares
{
    public class SmartResponseProcessor
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public SmartResponseProcessor(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<SmartResponseProcessor>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync($"Bad input parameters: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 409;
                await context.Response.WriteAsync($"We don't support this operation: {ex.Message}");
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 409;
                await context.Response.WriteAsync($"We don't support this operation: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync($"Unexpected error: {ex.Message}");
            }
        }
    }
}