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
            List<string> strings = new List<string>();

            while (input.IndexOf("<<") != -1)
            {

                int start = input.IndexOf("<<");
                input = input.Substring(start + 2);
                int end = input.IndexOf(">>");
                strings.Add(input.Substring(0, end));
                input = input.Substring(end + 2);
            }

            foreach (string s in strings)
            {
                Console.WriteLine(s);
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
