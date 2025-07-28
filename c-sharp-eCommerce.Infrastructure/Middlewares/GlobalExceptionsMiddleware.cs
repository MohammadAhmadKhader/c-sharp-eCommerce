using Microsoft.AspNetCore.Http;
using System.Text.Json;
using FluentValidation;

namespace c_sharp_eCommerce.Infrastructure.Middlewares
{
    public class GlobalExceptionsMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleFluentValidationException(context, ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                await HandleGeneralExceptionAsync(context, ex);
            }
        }

        private static Task HandleGeneralExceptionAsync(HttpContext context, Exception exception)
        {
            var genericMsg = "An unexpected error has occurred please try again later.";
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var result = JsonSerializer.Serialize(new
            {
                statusCode = 500,
                error = genericMsg,
            });

            return context.Response.WriteAsync(result);
        }

        private static Task HandleFluentValidationException(HttpContext context, ValidationException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 400;
            var errors = exception.Errors.Select(x => x.ErrorMessage).ToList();
            var result = JsonSerializer.Serialize(new
            {
                statusCode = 400,
                errors,
            });

            return context.Response.WriteAsync(result);
        }
    }
}
