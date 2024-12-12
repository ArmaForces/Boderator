using System.Threading.Tasks;
using ArmaforcesMissionBot.Helpers;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace ArmaforcesMissionBot.Modules
{
    public class LoggingModule : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<LoggingModule> _logger;

        public LoggingModule(ILogger<LoggingModule> logger)
        {
            _logger = logger;
        }

        [Command("change-log-level")]
        [Summary("Zmienia poziom logowania do czasu restartu bota")]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        public async Task ChangeLogLevel(LogLevel newLogLevel)
        {
            var oldLogLevel = LoggingHelper.LoggingLevelSwitch.MinimumLevel;
            if (oldLogLevel == (LogEventLevel) newLogLevel)
            {
                await ReplyAsync("New log level is the same as the old one");
            }
            else
            {
                _logger.LogInformation("Changed log level from {OldLogLevel} to {NewLogLevel}", oldLogLevel, newLogLevel.ToString());
                await ReplyAsync($"Changed log level from {oldLogLevel} to {newLogLevel}");
            }
            
            LoggingHelper.LoggingLevelSwitch.MinimumLevel = (LogEventLevel)newLogLevel;
        }
    }
}