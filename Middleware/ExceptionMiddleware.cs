using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Projeto_DIO___Catálogo_de_Jogos_com_DotNet.Middleware
{
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
            catch (System.Exception)
            {
                await HandleExceptionAsync(context);
            }
        }

        public static async Task HandleExceptionAsync (HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new { Message = "An error has occurred during your request. Please try again latter." });
        }     
    }
}