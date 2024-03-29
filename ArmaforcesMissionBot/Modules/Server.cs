﻿using System;
using System.Threading.Tasks;
using ArmaForces.ArmaServerManager.Discord.Features.Server;
using ArmaforcesMissionBot.Attributes;
using Discord.Commands;

namespace ArmaforcesMissionBot.Modules
{
    [Name("ArmaServerManager - Server")]
    public class Server : ServerModule
    {
        public Server(IServerManagerClient serverManagerClient) : base(serverManagerClient)
        {
        }

        [Summary("Sprawdza status serwera.")]
        [ContextDMOrChannel]
        public override Task ServerStatus() => base.ServerStatus();

        [Summary("Pozwala natychmiast uruchomić serwer z zadanym modsetem. Na przykład: AF!startServer default.")]
        [ContextDMOrChannel]
        public override Task StartServer(string modsetName) => base.StartServer(modsetName);
        
        [Command("startSerwer")]
        [Summary("Alias dla `AF!startServer`.")]
        [ContextDMOrChannel]
        public Task StartSerwer(string modsetName) => StartServer(modsetName);
        
        [Command("serverStart")]
        [Summary("Alias dla `AF!startServer`.")]
        [ContextDMOrChannel]
        public Task ServerStart(string modsetName) => StartServer(modsetName);

        [Summary("Pozwala uruchomić serwer z zadanym modsetem o zadanej godzinie w danym dniu. Na przykład: AF!startServer default 2020-07-17T19:00.")]
        [ContextDMOrChannel]
        public override Task StartServer(string modsetName, DateTime? dateTime) => base.StartServer(modsetName, dateTime);
        
        [Command("startSerwer")]
        [Summary("Alias dla `AF!startServer`.")]
        [ContextDMOrChannel]
        public Task StartSerwer(string modsetName, DateTime? dateTime) => StartServer(modsetName, dateTime);
        
        [Command("serverStart")]
        [Summary("Alias dla `AF!startServer`.")]
        [ContextDMOrChannel]
        public Task ServerStart(string modsetName, DateTime? dateTime) => StartServer(modsetName, dateTime);
    }
}
