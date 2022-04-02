using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ArmaForces.Boderator.BotService.Documentation
{
    internal static class DocumentationExtensions
    {
        private const string DefaultSwaggerJsonUrl = "/swagger/v3/swagger.json";

        public static IServiceCollection AddDocumentation(this IServiceCollection services, OpenApiInfo openApiConfig)
        {
            return services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc(openApiConfig.Version, openApiConfig);
                    options.EnableAnnotations();
                    options.UseAllOfToExtendReferenceSchemas();

                    var filePath = Path.Combine(System.AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                    options.IncludeXmlComments(filePath);
                });
        }

        public static IApplicationBuilder AddDocumentation(
            this IApplicationBuilder app,
            OpenApiInfo openApiConfig,
            string url = DefaultSwaggerJsonUrl)
        {
            return app.UseSwagger()
                .UseSwaggerUI(
                    options => options.SwaggerEndpoint(
                        url, 
                        openApiConfig.Title))
                .UseReDoc(
                    options =>
                    {
                        options.DocumentTitle = openApiConfig.Title;
                        options.ExpandResponses("");
                        options.SpecUrl = url;
                    });
        }
    }
}
