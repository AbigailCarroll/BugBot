using BugBot.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBot.Managers.Emotes
{
    internal static class Emote_Manager
    {
        public static Dictionary<string, string> Emotes = new Dictionary<string, string>();
        public static async Task LoadEmotes(string path)
        {
            StreamReader jsonString = new StreamReader(path);
            string json = await jsonString.ReadToEndAsync();
            Emotes = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            Console.WriteLine("Emotes Loaded");

            /*foreach (string key in Emotes.Keys)
            {
                Console.WriteLine(key + " : " + Emotes[key]);
            }*/ 
        }

        public static string GetEmote(string code)
        {
            if (Emotes.ContainsKey(code)) { return Emotes[code]; }
            return code;
        }
    }
}
