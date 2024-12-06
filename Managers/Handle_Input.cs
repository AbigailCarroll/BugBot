using BugBot.Objects.Search;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugBot.Managers
{
    internal static class Handle_Input
    {
        public static void Parse(string input, SocketMessage message)
        {
            string pattern = @"<<.+?>>";
            List<string> strings = new List<string>();

            foreach (Match match in Regex.Matches(input, pattern, RegexOptions.None, TimeSpan.FromSeconds(1)))
            {
                strings.Add(match.Value);
            }

            foreach (string s in strings)
            {
                message.Channel.SendMessageAsync($"Card Name Recieved: {s}");
                string findOutput = Trie.Find(s);
                if (findOutput != "")
                {
                    Console.WriteLine("Find Output: " + findOutput);
                    message.Channel.SendMessageAsync(findOutput);
                }
                else 
                {
                    Console.WriteLine("card not found");
                    message.Channel.SendMessageAsync("Card Not Found");
                }

                //message.Channel.SendMessageAsync(findOutput);
            }
        }

        public static string Clean_Input(string cardName)
        {
            cardName = Regex.Replace(cardName, @"&", "and");
            cardName = Regex.Replace(cardName, @"[^a-zA-Z]", ""); //removes any non alphabetic characters from the card name

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(cardName);
            cardName = System.Text.Encoding.UTF8.GetString(tempBytes);
            return cardName;
        }


    }
}
