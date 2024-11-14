using ArmaforcesMissionBot.DataClasses;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.Discord;

namespace ArmaforcesMissionBot.Helpers
{
    internal class LoggingHelper
    {
        private const string OutputTemplate = "{Timestamp:yyyy-MM-ddTHH:mm:ss.fffzzz} [{Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}";
        
        public static readonly LoggingLevelSwitch LoggingLevelSwitch = new LoggingLevelSwitch();

        public static ILogger CreateSerilogLogger()
        {
            return CreateLoggerConfiguration()
                .CreateLogger();
        }

        private static LoggerConfiguration CreateLoggerConfiguration()
        {
            var config = new Config();
            config.Load();

            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: OutputTemplate);

            if (config.LogWebhookId.HasValue && config.LogWebhookToken != null)
            {
                loggerConfiguration.WriteTo.Discord(
                    webhookId: config.LogWebhookId.Value,
                    webhookToken: config.LogWebhookToken,
                    restrictedToMinimumLevel: config.LogWebhookLevel);
            }

            return loggerConfiguration;
        }
    }
}