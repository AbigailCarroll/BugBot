using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using BugBot.Objects;
using BugBot.Objects.Search;
using BugBot.Managers.Database;
using System.Security.Cryptography.Xml;

namespace BugBot.Managers
{
    public static class Card_Importer
    {//itemsPerPage maimum is 36 greater values cause no bugs (that i've seen) but have no effect
        public static async void ImportCards()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Console.WriteLine("Starting Card Import");
            string pageLength = "36";
            string language = "en-us"; //for whatever reason en-en language (british english?) sends invalid links for the imagepath field.
            string root = "https://api.altered.gg/cards";
            

            int page = 1;
            int totalItems = -1;
            int finalTotalItems = -1;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(root);

            List<Card> allCards = new List<Card>();

            while (totalItems != 0)
            {
                string parameters = $"?itemsPerPage={pageLength}&locale={language}&page={page}";
                HttpResponseMessage response = await client.GetAsync(parameters).ConfigureAwait(false);
                

                if (response.IsSuccessStatusCode)
                {


                    

                    //var jsonString = await response.Content.ReadAsStringAsync();
                    //API_Response cards = JsonConvert.DeserializeObject<API_Response>(jsonString);

                    string jsonString = await response.Content.ReadAsStringAsync();

                    API_Response cards = JsonConvert.DeserializeObject<API_Response>(jsonString);
                    if(totalItems > finalTotalItems)
                    {
                        finalTotalItems = totalItems;
                    }

                    totalItems = cards.getTotalItems();

                    if (cards != null)
                    {
                        foreach (Card c in cards.getMembers())
                        {
                            c.CleanPowersandCosts();
                            c.setRulings();
                            allCards.Add(c);
                        }
                    }
                    else
                    {
                        Console.WriteLine("cards was null");
                    }

                }
                else
                {
                    Console.WriteLine("Bad Response, Error as Follows:");
                    Console.WriteLine(response.StatusCode);
                }
                page++;

            }

            
            Database_Handler.addCards(allCards);
            Console.WriteLine($"Import Done, Total number of cards imported is {allCards.Count()}, this should be equal to {finalTotalItems}");
            
            watch.Stop();
            Console.WriteLine("Import Time: " + watch.ElapsedMilliseconds + "ms");





        }

        public static async Task<Card> ImportUnique(string reference)
        {
            Card unique = null;
           

            string language = "en-en";

            string parameters = "?" + language;
            string root = "https://api.altered.gg/cards/" + reference;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(root);

            HttpResponseMessage response = await client.GetAsync(parameters).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();

                unique = JsonConvert.DeserializeObject<Card>(jsonString);

                unique.CleanPowersandCosts();
                //unique.setRulings(); as of now unique cards do not have rulings in the API
                Database_Handler.addCard(unique);
                return unique;
            }

            return null;
        }


        public static async Task<Card> ImportUnique(string name, string number) //takes the name of the card and the unique number in the form "U_0000"
        {
            string reference = "";
            if (Database_Handler.getCard(name, "C") == null)
            {
                return null;
            }
            reference = Database_Handler.getCard(name, "C").getReference();
            reference = reference.Remove(reference.Length - 1);
            reference += number;

            return ImportUnique(reference).Result;
        }
    }
}
