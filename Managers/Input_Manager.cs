using BugBot.Managers.Database;
using BugBot.Objects.Search;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugBot.Managers
{
    internal static class Input_Manager
    {
        public static HashSet<Embed> Parse(string input)
        {
            string pattern = @"<<(.+?)>>";

            HashSet<Embed> embeds = new HashSet<Embed>();

            foreach (Match match in Regex.Matches(input, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(50)))
            {
                string[] tokens = match.Groups[1].Captures[0].Value.Split("|");
                
                /*foreach (string s in tokens)
                {
                    Console.WriteLine(s);
                }*/
                if (tokens.Length < 1 || string.IsNullOrEmpty(tokens[0]))
                {
                    Console.WriteLine("No Tokens Provided");
                }
                else
                {
                    Embed embed = Embed_Manager.Embed_Response(tokens);
                    embeds.Add(embed);
                    //message.Channel.SendMessageAsync(embed: embed, messageReference: new MessageReference(message.Id));
                    
                }
            }
            return embeds;
        }

        public static string Clean_Input(string cardName)
        {
            cardName = Regex.Replace(cardName, @"&", "and");
            cardName = Regex.Replace(cardName, @"[^a-zA-Z]", ""); //removes any non alphabetic characters from the card name

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(cardName); 
            cardName = System.Text.Encoding.UTF8.GetString(tempBytes);
            cardName = cardName.ToLower();
            return cardName;
        }


    }
}
