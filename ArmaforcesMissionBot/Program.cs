using System;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using ArmaforcesMissionBot.Helpers;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ArmaforcesMissionBot
{
    public class Program
    {
        
        public static async Task Main(string[] args)
        {
            Log.Logger = LoggingHelper.CreateSerilogLogger();

            try
            {
                await CreateHostBuilder(args).Build().RunAsync();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Boderator terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(
                    webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
    }
}
