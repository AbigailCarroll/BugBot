using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugBot.config;
using Discord.WebSocket;
using BugBot.Managers;

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
            //remove unecessary privileges when done

            client = new DiscordSocketClient(config);
            
            client.MessageReceived += MessageReceived;
            client.Log += Log;


            var token = jsonReader.token;

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            Console.WriteLine("bot online!");

            Card_Importer.ImportCards();

            await Task.Delay(-1);
        }

        private static async Task MessageReceived(SocketMessage message)
        {
            if (message.Author.IsBot) { return; }
            //await message.Channel.SendMessageAsync("Hello!");
            Console.WriteLine("Message Content: " + message.Content);
            Handle_Input.Parse(message.Content, message);
            return;
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

    }
}
