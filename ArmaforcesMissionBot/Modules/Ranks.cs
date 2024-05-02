using ArmaforcesMissionBot.Attributes;
using ArmaforcesMissionBot.DataClasses;
using ArmaforcesMissionBot.Helpers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ArmaforcesMissionBot.DataClasses.OpenedDialogs;
using System.Web;

namespace ArmaforcesMissionBot.Modules
{
    [Name("Rangi")]
    public class Ranks : ModuleBase<SocketCommandContext>
    {
        public IServiceProvider _map { get; set; }
        public DiscordSocketClient _client { get; set; }
        public Config _config { get; set; }
        public MiscHelper _miscHelper { get; set; }
        public OpenedDialogs _dialogs { get; set; }

        [Command("rekrutuj")]
        [Summary("Przydziela rangę rekrut.")]
        [RequireRank(RanksEnum.Recruiter)]
        public async Task Recruit(IGuildUser user)
        {
            Console.WriteLine($"[{DateTime.Now.ToString()}] {Context.User.Username} called recruit command");
            var signupRole = Context.Guild.GetRole(_config.SignupRole);
            if (user.RoleIds.Contains(_config.RecruitRole))
                await ReplyAsync($"Przecież {user.Mention} już został zrekrutowany.");
            else if (user.RoleIds.Contains(_config.SignupRole))
                await ReplyAsync($"Przecież {user.Mention} jest już w {signupRole.Mention}!");
            else
            {
                await user.AddRoleAsync(Context.Guild.GetRole(_config.RecruitRole));
                var recruitMessageText =
                    $"Gratulujemy przyjęcia {user.Mention} w grono rekrutów! Od teraz masz miesiąc na rozegranie swojej pierwszej misji z nami, wtedy otrzymasz rangę #{signupRole.Name}#! W innym wypadku zostaniesz usunięty z Discorda z możliwością powrotu.\n" +
                    $"Polecamy też sprawdzić kanał {Context.Guild.GetTextChannel(_config.RecruitInfoChannel).Mention}.\n" +
                    $"W razie pytań pisz na {Context.Guild.GetTextChannel(_config.RecruitAskChannel).Mention}.\n" +
                    $"Twoim opiekunem do momentu dołączenia do grupy jest {Context.User.Mention}.";
                var recruitMessage = await ReplyAsync(recruitMessageText);
                // Modify message to include rank mention but without mentioning it
                var replacedMessage = recruitMessage.Content;
                replacedMessage = Regex.Replace(replacedMessage, "#ArmaForces#", $"{signupRole.Mention}");
                await recruitMessage.ModifyAsync(x => x.Content = replacedMessage);
            }

            await Context.Message.DeleteAsync();
        }

        [Command("wyrzuc")]
        [Summary("Wyrzuca rekruta bądź randoma z Discorda.")]
        [RequireRank(RanksEnum.Recruiter)]
        public async Task Kick(IGuildUser user)
        {
            Console.WriteLine($"[{DateTime.Now.ToString()}] {Context.User.Username} called kick command");
            var signupRole = Context.Guild.GetRole(_config.SignupRole);
            var userRoleIds = user.RoleIds;
            if (userRoleIds.All(x => x == _config.RecruitRole || x == _config.AFGuild))
            {
                var embedBuilder = new EmbedBuilder()
                {
                    ImageUrl = _config.KickImageUrl
                };
                var replyMessage =
                    await ReplyAsync(
                        $"{user.Mention} został pomyślnie wykopany z serwera przez {Context.User.Mention}.",
                        embed: embedBuilder.Build());
                await user.KickAsync("AFK");
                _ = Task.Run(async () =>
                {
                    await Task.Delay(5000);
                    await replyMessage.ModifyAsync(x => x.Embed = null);
                });
            }
            else
            {
                await ReplyAsync($"Nie możesz wyrzucić {user.Mention}, nie jest on rekrutem.");
            }

            await Context.Message.DeleteAsync();
        }

        [Command("ranga")]
        [Summary("Dodaj wiadomość do automatycznego dodwania rangi. " +
                 "Argumenty: tytuł; opis; rangi do nadania; reakcje.")]
        [RequireRank(RanksEnum.RoleMaker)]
        public async Task RankAssign([Remainder] string rankText)
        {
            var spilttedTexts = rankText.Split(";");
            if (spilttedTexts.Length != 4)
            {
                await ReplyAsync("Źle!");
                return;
            }
            var rankTitle = spilttedTexts[0];
            var rankDescription = spilttedTexts[1];
            var rankType = spilttedTexts[2];
            var reaction = spilttedTexts[3];

            var rankMatches = MiscHelper.GetRankMatchesFromText(rankType);
            var reactionsMatches = MiscHelper.GetEmojiMatchesFromText(reaction);

            if (reactionsMatches.Distinct().Count() != reactionsMatches.Length)
            {
                await ReplyAsync("Reakcje nie są unikalne!");
                return;
            }

            if (rankMatches.Count != reactionsMatches.Length)
            {
                if (rankMatches.Count > reactionsMatches.Length)
                {
                    await ReplyAsync("Za mało reakcji do rang!");
                    return;
                }

                reactionsMatches = reactionsMatches[0..rankMatches.Count];
            }

            var embedBuilder = new EmbedBuilder();

            string embedText = rankDescription + "\r\nReakcje:\r\n";

            foreach (var (reactionMatch, i) in rankMatches.Select((value, i) => (value, i)))
            {
                embedText = embedText + reactionsMatches[i] + " - ";
                embedText = embedText + reactionMatch.Value + "\r\n";
            }

            embedBuilder.WithTitle(rankTitle).WithDescription(embedText);

            _miscHelper.CreateConfirmationDialog(
                        _dialogs,
                       Context,
                       embedBuilder.Build(),
            async dialog =>
            {
                _dialogs.Dialogs.Remove(dialog);
                ReplyAsync("Pasi!");
                
                var channel = Context.Guild.GetTextChannel(_config.RoleAssignChannel);
                var message = await channel.SendMessageAsync(embed: embedBuilder.Build());
                await message.AddReactionsAsync(reactionsMatches);
            },
            dialog =>
            {
                Context.Channel.DeleteMessageAsync(dialog.DialogID);
                _dialogs.Dialogs.Remove(dialog);
                ReplyAsync("Nie to nie!");
                return;
            });
        }
    }
}
