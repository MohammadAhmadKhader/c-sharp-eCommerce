using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation.Results;
using FluentValidation;

namespace c_sharp_eCommerce.Infrastructure.Middlewares
{
	public class GlobalExceptionsMiddleware
	{
		private readonly RequestDelegate next;

        public GlobalExceptionsMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
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
