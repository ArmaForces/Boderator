using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ArmaForces.Boderator.BotService.Configuration;
using ArmaForces.Boderator.BotService.Documentation;
using ArmaForces.Boderator.BotService.Features.DiscordClient.Infrastructure.DependencyInjection;
using ArmaForces.Boderator.Core.DependencyInjection;
using Discord.WebSocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ArmaForces.Boderator.BotService
{
    public class Startup
    {
        private OpenApiInfo OpenApiConfiguration { get; } = new()
        {
            Title = "ArmaForces Boderator API",
            Description = "API that does nothing. For now.",
            Version = "v3",
            Contact = new OpenApiContact
            {
                Name = "ArmaForces",
                Url = new Uri("https://armaforces.com")
            }
        };

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDocumentation(OpenApiConfiguration);
            services.AddBoderatorCore(serviceProvider => serviceProvider.GetRequiredService<BoderatorConfiguration>().ConnectionString);
            services.AddSingleton(_ => new BoderatorConfigurationFactory().CreateConfiguration());
            services.AddDiscordClient();
            services.AutoAddInterfacesAsScoped(typeof(Startup).Assembly);

            services.AddMvc(options => options
                .Filters.Add(new ExceptionFilter()))
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    opt.JsonSerializerOptions.WriteIndented = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.AddDocumentation(OpenApiConfiguration);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class ExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is not ArgumentNullException) return;
            
            var error = new
            {
                Message = "Validation error",
                Details = context.Exception.Message
            };

            context.Result = new BadRequestObjectResult(error);
            context.ExceptionHandled = true;
        }
    }
}
