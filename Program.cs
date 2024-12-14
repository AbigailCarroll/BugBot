using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugBot.config;
using Discord.WebSocket;
using BugBot.Managers;
using BugBot.Objects;

namespace BugBot
{
    class Program
    {
        
        private static DiscordSocketClient client;

        
        static async Task Main(string[] args)
        {
            var jsonReader = new JSONReader();
            await jsonReader.ReadJSON();

            var config = new DiscordSocketConfig { GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent};
           

            client = new DiscordSocketClient(config);
            
            client.MessageReceived += MessageReceived;
            client.Log += Log;


            var token = jsonReader.token;

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            Card_Importer.ImportCards();

            Console.WriteLine("bot online!");

            

            await Task.Delay(-1);
        }

        private static async Task MessageReceived(SocketMessage message)
        {
            if (message.Author.IsBot) { return; }
            try
            {
                HashSet<Embed> embeds = Input_Manager.Parse(message.Content);
                foreach (Embed embed in embeds)
                {
                    await message.Channel.SendMessageAsync(embed: embed, messageReference: new MessageReference(message.Id));
                }

            }
            catch (Exception e) 
            {
                var jsonReader = new JSONReader();
                await jsonReader.ReadJSON();

                Console.WriteLine("ERROR "+ DateTime.Now+":  " + e.Message);
                await client.GetGuild(1306971465220100169).GetTextChannel(1316248768034639972).SendMessageAsync("Error: " + e.Message);

                Embed embed = Embed_Manager.UnhandledError();
                await message.Channel.SendMessageAsync(embed: embed,messageReference: new MessageReference(message.Id));
            }
            return;
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

    }
}



/*
 TODO: 
 * Implement Embeds to display info nicely on the discord side: https://docs.discordnet.dev/api/Discord.Embed.html?q=embed DONE
 * Implement autocorrect to go along with autocomplete: https://medium.com/@willsentance/how-to-write-your-own-spellchecker-and-autocorrect-algorithm-in-under-80-lines-of-code-6d65d21bb7b6
 * ^ need to come up with a way to make those two work together, for instance autocomplete as is doesn't work when there is a typo and 
 *   this autocorrect method wouldn't work on imcomplete card names
 *   
 * Implement foreign language support
 * Make SQL database to host card info rather than calling from the database on startup, not only is this bad for consuming api resources but
 * ^ would be good to have is the API changes/goes down.
 * 
 * 
 * Add a seperate branch in the trie for cards beginning with "the", so "The Frog Prince" can be searched for with the input "Frog Prince"
 * 
 * 
 * */
