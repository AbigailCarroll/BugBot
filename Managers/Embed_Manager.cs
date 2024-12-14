using BugBot.Managers.Database;
using BugBot.Objects;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugBot.Managers
{
    internal static class Embed_Manager
    {
        static Dictionary<string, Color> rarityColour = new Dictionary<string, Color>() { 
            { "C", Color.DarkGrey },
            { "R", Color.Blue },
            { "U", Color.Gold}
        };
        public static Embed Embed_Response(string[] tokens)
        {
            string language = "en-us";
            string rarity = "C";
            Card card;

            



            for (int i = 0; i < tokens.Length; i++)
            {
                tokens[i] = tokens[i].Replace(" ", "");
            }
            for (int i = 1; i < tokens.Length; i++)
            {
                tokens[i] = tokens[i].ToUpper();
                switch (tokens[i])
                {
                    case "C":
                        rarity = "C";
                        break;
                    case "R1":
                    case "R":
                        rarity = "R1";
                        break;
                    case "R2":
                    case "OOC":
                    case "OOF":
                    case "CS":
                        rarity = "R2";
                        break;
                    case "FR":
                        language = "fr-fr";
                        break;
                    case "DE":
                        language = "de-de";
                        break;
                    case "ES":
                        language = "es-es";
                        break;
                    case "IT":
                        language = "it-it";
                        break;
                    default:
                        if (Regex.Match(tokens[i], "U\\d").Success)
                        {
                            break;
                            //handle uniques, skip for now
                        }
                        else
                        {
                            return InvalidToken(tokens[i]);
                        }
                }

            }
            
            string uncleanName = tokens[0];
            tokens[0] = Input_Manager.Clean_Input(tokens[0]);

            

            if (Database_Handler.getCardByReference(uncleanName) == null)
            {
                string foundName = Trie.Find(tokens[0]);
                if (Database_Handler.getCard(foundName, rarity) == null)
                {
                    return CardNotFound(uncleanName, rarity);
                }
                else
                {
                    card = Database_Handler.getCard(foundName, rarity);
                }
            }
            else
            {
                card = Database_Handler.getCardByReference(uncleanName);
            }
            if (card == null)
            {
                return CardNotFound(uncleanName, rarity);
            }

            


           




            return BuildEmbed(card, language);
        }

        

        private static Embed CardNotFound(string cardName, string rarity) 
        {
            EmbedBuilder embed = new EmbedBuilder();

            embed.Title = "Card Not Found";
            embed.Color = Color.Red;
            embed.Description = $"Card \"{cardName}\" with rarity \"{rarity}\"was not found. \n Check your spelling and try again.";

            return embed.Build();
        }

        private static Embed InvalidToken(string token)
        {
            EmbedBuilder embed = new EmbedBuilder();
            string url = "https://github.com/AbigailCarroll/BugBot/blob/master/README.md";
            embed.Title = "Invalid Token";
            embed.Color = Color.Red;
            embed.Description = $"Token >{token} is invalid. \n For a list of valid tokens visit the GitHub page at: {url}";

            return embed.Build();
        }

        public static Embed UnhandledError()
        {
            EmbedBuilder embed = new EmbedBuilder();

            embed.Title = "Unhandled Exception";
            embed.Color = Color.Red;
            embed.Description = "Something went wrong, this error has been reported. \n This bot doesn't store user messages, if the problem persists create a new issue on the github page at https://github.com/AbigailCarroll/BugBot/issues with the message that triggered this.";

            return embed.Build() ;
        }

        private static Embed BuildEmbed(Card card, string language)
        {
            bool inline = true;
            EmbedBuilder embed = new EmbedBuilder();

            embed.WithTitle(card.getName())
                .WithUrl("https://www.altered.gg/cards/" + card.getReference())
                .WithThumbnailUrl(card.getImagePath())
                .WithDescription(card.getEffects())
                .AddField("Type", card.getCardType(), inline)
                .AddField("Faction", card.getFaction(), inline);
           
                  
            string cardType = card.getCardType();

            if (card.getCardType() != "Hero")
            {
                embed.AddField("Rarity", card.getRarity(), inline);
                embed.AddField("Costs", card.getCosts(), inline);
            }

            else 
            {
                embed.AddField("Reserve Slots", card.getReserveSlots(), inline);
                embed.AddField("Landmark Slots", card.getLandmarkSlots(), inline);
            }
            try
            {
            
                if (!string.IsNullOrEmpty(card.getPowers()))
                {
                    embed.AddField("Powers", card.getPowers(), inline);
                }
            }
            catch (Exception e) { Console.WriteLine("Error with cards without powers"); }
            

            if(card.getRarity() == "Rare")
            {
                embed.WithColor(Color.Blue);
            }
            else if (card.getRarity() == "Unique")
            {
                embed.WithColor(Color.Gold);
            }


            return embed.Build() ;
        }

        

        

    }
}
