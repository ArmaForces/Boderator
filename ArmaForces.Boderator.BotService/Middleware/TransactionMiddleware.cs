using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;

namespace ArmaForces.Boderator.BotService.Middleware
{
    internal class TransactionMiddleware
    {
        private readonly RequestDelegate _next;

        public TransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method != "GET")
            {
                using var transaction = new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.ReadCommitted
                    },
                    TransactionScopeAsyncFlowOption.Enabled);

                await _next.Invoke(context);

                transaction.Complete();
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
