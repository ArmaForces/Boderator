using ArmaforcesMissionBot.DataClasses;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using ArmaforcesMissionBot.Helpers;

namespace ArmaforcesMissionBot.Handlers
{
    public class SignupHandler : IInstallable
    {
        private DiscordSocketClient _client;
        private MiscHelper _miscHelper;
        private IServiceProvider _services;
        private Config _config;

        public async Task Install(IServiceProvider map)
        {
            _client = map.GetService<DiscordSocketClient>();
            _config = map.GetService<Config>();
            _miscHelper = map.GetService<MiscHelper>();
            _services = map;
            // Hook the MessageReceived event into our command handler
            _client.ChannelDestroyed += HandleChannelRemoved;

        }

        private async Task HandleChannelRemoved(SocketChannel channel)
        {
            var signups = _services.GetService<SignupsData>();

            if (signups.Missions.Any(x => x.SignupChannel == channel.Id))
            {
                signups.Missions.RemoveAll(x => x.SignupChannel == channel.Id);
            }
        }

    }
}
