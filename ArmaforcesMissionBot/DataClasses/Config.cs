#nullable enable
using dotenv.net;
using System;
using System.Reflection;
using Serilog;
using Serilog.Events;

namespace ArmaforcesMissionBot.DataClasses
{
    public class Config
    {
        public string DiscordToken { get; set; }
        public ulong SignupsCategory { get; set; }
        public ulong SignupsArchive { get; set; }
        public ulong AFGuild { get; set; }
        public ulong MissionMakerRole { get; set; }
        public ulong SignupRole { get; set; }
        public ulong BotRole { get; set; }
        public ulong RecruiterRole { get; set; }
        public ulong RecruitRole { get; set; }
        
        /// <summary>
        /// Discord webhook ID for log messages.
        /// </summary>
        public ulong? LogWebhookId { get; set; }
        
        /// <summary>
        /// Discord webhook token for log messages.
        /// </summary>
        public string? LogWebhookToken { get; set; }

        /// <summary>
        /// Minimum level that will be logged to Discord webhook.
        /// </summary>
        public LogEventLevel LogWebhookLevel { get; set; } = LogEventLevel.Error;
        
        public string KickImageUrl { get; set; }
        public string BanImageUrl { get; set; }
        public string ServerManagerUrl { get; set; }
        public string ServerManagerApiKey { get; set; }
        public string ModsetsApiUrl { get; set; }
        public ulong CreateMissionChannel { get; set; }
        public ulong PublicContemptChannel { get; set; }
        public ulong HallOfShameChannel { get; set; }
        public ulong RecruitInfoChannel { get; set; }
        public ulong RecruitAskChannel { get; set; } 

        public void Load()
        {
            DotEnv.Config(false);

            PropertyInfo[] properties = typeof(Config).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in properties)
            {
                if(propertyInfo.PropertyType == typeof(string))
                    TrySetStringValue(propertyInfo);
                else if (propertyInfo.PropertyType == typeof(ulong) || propertyInfo.PropertyType == typeof(ulong?))
                    TrySetUlongValue(propertyInfo);
                else if (propertyInfo.PropertyType == typeof(LogEventLevel))
                    TrySetEnumValue<LogEventLevel>(propertyInfo);
            }
        }

        private void TrySetEnumValue<T>(PropertyInfo propertyInfo) where T : struct
        {
            var stringValue = GetVariable(propertyInfo.Name);
            if (stringValue != null && Enum.TryParse(stringValue, out T valueToSet))
            {
                propertyInfo.SetValue(this, valueToSet);
            }
        }

        private void TrySetStringValue(PropertyInfo propertyInfo)
        {
            var stringValue = GetVariable(propertyInfo.Name);
            if (stringValue != null)
            {
                propertyInfo.SetValue(this, stringValue);
            }
        }

        private void TrySetUlongValue(PropertyInfo propertyInfo)
        {
            var stringValue = GetVariable(propertyInfo.Name);
            if (stringValue != null && ulong.TryParse(stringValue, out var valueToSet))
            {
                propertyInfo.SetValue(this, valueToSet);
            }
        }

        private static string? GetVariable(string variableName)
        {
            try
            {
                return Environment.GetEnvironmentVariable("AF_" + variableName);
            }
            catch (Exception exception)
            {
                Log.Warning(exception, "Failed to read environment variable {Name}", variableName);
                return null;
            }
        }
    }
}
