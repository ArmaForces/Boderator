using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ArmaForces.Boderator.BotService.Middleware
{
    public class TransactionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // context.RequestServices.GetRequiredService()
        }
    }
}
