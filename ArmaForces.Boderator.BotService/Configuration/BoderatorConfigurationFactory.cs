using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace ArmaForces.Boderator.BotService.Configuration
{
    internal class BoderatorConfigurationFactory : IBoderatorConfigurationFactory
    {
        private readonly IDictionary _environmentVariables;

        public BoderatorConfigurationFactory()
        {
            _environmentVariables = Environment.GetEnvironmentVariables();
        }

        public BoderatorConfigurationFactory(IConfiguration configuration)
        {
            _environmentVariables = new Dictionary<string, object>
            {
                {$"AF_Boderator_{nameof(BoderatorConfiguration.ConnectionString)}", configuration.GetConnectionString("DefaultConnection")},
                {$"AF_Boderator_{nameof(BoderatorConfiguration.DiscordToken)}", ""}
            };
        }
        
        // TODO: Consider making this a bit more automatic so configuration is easily extensible
        public BoderatorConfiguration CreateConfiguration() => new BoderatorConfiguration
        {
            ConnectionString = GetStringValue(nameof(BoderatorConfiguration.ConnectionString)),
            DiscordToken = GetStringValue(nameof(BoderatorConfiguration.DiscordToken))
        };

        private string GetStringValue(string variableName)
        {
            var fullVariableName = $"AF_Boderator_{variableName}";
            var value = _environmentVariables[fullVariableName];

            return value is not null
                ? (string) value
                : throw new ConfigurationErrorsException($"Variable {fullVariableName} does not exist.");
        }
    }

    internal interface IBoderatorConfigurationFactory
    {
        BoderatorConfiguration CreateConfiguration();
    }
}
