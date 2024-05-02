using ArmaforcesMissionBot.DataClasses;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using static ArmaforcesMissionBot.DataClasses.OpenedDialogs;

namespace ArmaforcesMissionBot.Helpers
{
    public class MiscHelper
    {
        private readonly DiscordSocketClient _client;
        private readonly Config _config;

        public MiscHelper(DiscordSocketClient client, Config config)
        {
            _client = client;
            _config = config;
        }

        public List<string> BuildTeamSlots(ArmaforcesMissionBotSharedClasses.Mission.Team team)
        {
            List<string> results = new List<string>();
            results.Add("");
            foreach (var slot in team.Slots)
            {
                for (var i = 0; i < slot.Count; i++)
                {
                    string description = $"{HttpUtility.HtmlDecode(slot.Emoji)}";
                    if (slot.Name != "" && i == 0)
                        description += $"({slot.Name})";
                    description += "-";
                    if (i < slot.Signed.Count)
                    {
                        var user = _client.GetGuild(_config.AFGuild).GetUser(slot.Signed.ElementAt(i));
                        if(user != null)
                            description += user.Mention;
                    }
                    description += "\n";
                    
                    if(results.Last().Length + description.Length > 1024)
                        results.Add("");

                    results[results.Count-1] += description;
                }
            }

            /*foreach (var prebeton in team.Signed)
            {
                Console.WriteLine(prebeton.Value + " " + prebeton.Key);
                Console.WriteLine(HttpUtility.HtmlDecode(prebeton.Value) + " " + HttpUtility.HtmlDecode(prebeton.Key));
                var regex = new Regex(Regex.Escape(HttpUtility.HtmlDecode(prebeton.Value)) + @"-(?:$|\n)");
                description = regex.Replace(description, HttpUtility.HtmlDecode(prebeton.Value) + "-" + HttpUtility.HtmlDecode(prebeton.Key) + "\n", 1);
            }*/

            return results;
        }

        public void BuildTeamsEmbed(List<ArmaforcesMissionBotSharedClasses.Mission.Team> teams, EmbedBuilder builder, bool removeSlotNamesFromName = false)
        {
            foreach (var team in teams)
            {
                var slots = BuildTeamSlots(team);

                var teamName = team.Name;
                if (removeSlotNamesFromName)
                {
                    foreach (var slot in team.Slots)
                    {
                        if (teamName.Contains(slot.Emoji))
                            teamName = teamName.Remove(teamName.IndexOf(slot.Emoji));
                    }
                }

                if(slots.Count == 1)
                    builder.AddField(teamName, slots[0], true);
                else if(slots.Count > 1)
                {
                    foreach(var part in slots)
                    {
                        builder.AddField(teamName, part, true);
                    }
                }
            }
        }

        public static string BuildEditTeamsPanel(List<ArmaforcesMissionBotSharedClasses.Mission.Team> teams, int highlightIndex)
        {
            string result = "";

            int index = 0;
            foreach (var team in teams)
            {
                if (highlightIndex == index)
                    result += "**";
                result += $"{team.Name}";
                if (highlightIndex == index)
                    result += "**";
                result += "\n";
                index++;
            }

            return result;
        }

        public static int CountFreeSlots(ArmaforcesMissionBotSharedClasses.Mission mission)
        {
            return CountAllSlots(mission) - mission.SignedUsers.Count;
        }

        public static int CountAllSlots(ArmaforcesMissionBotSharedClasses.Mission mission)
        {
            int slots = 0;
            foreach (var team in mission.Teams)
            {
                foreach (var slot in team.Slots)
                {
                    slots += slot.Count;
                }
            }

            return slots;
        }

        public async void CreateConfirmationDialog(
            OpenedDialogs openedDialogs,
            SocketCommandContext context,
            Embed description,
            Action<Dialog> confirmAction,
            Action<Dialog> cancelAction)
        {
            var dialog = new Dialog();

            var message = await context.Channel.SendMessageAsync("Zgadza sie?", embed: description);

            dialog.DialogID = message.Id;
            dialog.DialogOwner = context.User.Id;
            dialog.Buttons["✔️"] = confirmAction;
            dialog.Buttons["❌"] = cancelAction;

            var reactions = new List<IEmote>();
            foreach(var key in dialog.Buttons.Keys)
            {
                reactions.Add(new Emoji(key));
            }
            await message.AddReactionsAsync(reactions.ToArray());

            openedDialogs.Dialogs.Add(dialog);
        }

        public static MatchCollection GetSlotMatchesFromText(string text)
        {
            string unicodeEmoji = @"(?:\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])(?:\ufe0f)?";
            string emote = $@"((?:<?a?:.+?:(?:[0-9]+>)?)|{unicodeEmoji})";
            string slotCount = @"(\[[0-9]+\])";
            string slotName = @"([^\|]*?)?";
            string rolePattern = $@"[ ]*{emote}[ ]*{slotCount}[ ]*{slotName}[ ]*(?:\|)?";

            return Regex.Matches(text, rolePattern, RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
        }

        public static List<Tuple<string, string>> GetRankArrayMatchesFromText(string text)
        {
            string unicodeEmoji = @"(?:\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])(?:\ufe0f)?";
            string emote = $@"((?:<?a?:.+?:(?:[0-9]+>)?)|{unicodeEmoji})";
            string rank = @"\<\@\&([0-9]+)\>";
            string rolePattern = $@"[ ]*{emote}[ ]*-[ ]*{rank}";

            var rankMatches = Regex.Matches(text, rolePattern, RegexOptions.IgnoreCase);

            var rankList = new List<Tuple<string, string>> { };

            foreach (Match rankLine in rankMatches)
            {
                var spilttedTexts = rankLine.Value.Split(" - ");
                rankList.Add(Tuple.Create(spilttedTexts[0], spilttedTexts[1]));
            }

            return rankList;
        }

        public static IEmote[] GetEmojiMatchesFromText(string text)
        {
            string unicodeEmoji = @"(?:\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])(?:\ufe0f)?";
            string emote = $@"((?:<?a?:.+?:(?:[0-9]+>)?)|{unicodeEmoji})";

            var emojiMatches = Regex.Matches(text, emote, RegexOptions.IgnoreCase);

            IEmote[] emoji = new IEmote[emojiMatches.Count];

            foreach (var (emojiMatch, i) in emojiMatches.Select((value, i) => (value, i)))
            {
                try
                {
                    emoji[i] = Emote.Parse(HttpUtility.HtmlDecode(emojiMatch.Value));
                }
                catch (Exception e)
                {
                    emoji[i] = new Emoji(HttpUtility.HtmlDecode(emojiMatch.Value));
                }
            }

            return emoji;
        }

        public static MatchCollection GetRankMatchesFromText(string text)
        {
            string rank = @"\<\@\&([0-9]+)\>";

            return Regex.Matches(text, rank, RegexOptions.IgnoreCase);
        }
    }
}
