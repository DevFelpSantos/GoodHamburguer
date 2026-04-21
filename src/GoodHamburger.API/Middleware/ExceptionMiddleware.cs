namespace GoodHamburger.API.Middlewares
{
    using FluentValidation;
    using System.Net;

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
            catch (ValidationException ex)
            {
                await HandleException(context, HttpStatusCode.BadRequest, ex.Errors.Select(e => e.ErrorMessage));
            }
            catch (KeyNotFoundException ex)
            {
                await HandleException(context, HttpStatusCode.NotFound, new[] { ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleException(context, HttpStatusCode.Unauthorized, new[] { ex.Message });
            }
            catch (Exception)
            {
                await HandleException(context, HttpStatusCode.InternalServerError, new[] { "Erro interno no servidor" });
            }
        }

        private static async Task HandleException(HttpContext context, HttpStatusCode statusCode, IEnumerable<string> errors)
        {
            context.Response.StatusCode = (int)statusCode;
            var response = new
            {
                success = false,
                errors
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}