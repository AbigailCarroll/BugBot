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
        public static List<string> Parse(string input, SocketMessage message)
        {
            string pattern = @"<<.+?>>";
            List<string> strings = new List<string>();

            foreach (Match match in Regex.Matches(input, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(50)))
            {
                strings.Add(match.Value);
            }

            return strings;


           
        }

        public static void VerifyCardName(string name, SocketMessage message)
        {
            string verified = Trie.Find(name);
            if (verified != "")
            {
                Console.WriteLine("Find Output: " + verified);
                message.Channel.SendMessageAsync(verified);
                message.Channel.SendMessageAsync(Database_Handler.getCard(verified, 0).getImagePath());
            }
            else
            {
                Console.WriteLine("card not found");
                message.Channel.SendMessageAsync("Card Not Found");
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
            cardName = cardName.ToLower();
            return cardName;
        }


    }
}
