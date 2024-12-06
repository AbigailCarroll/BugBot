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

namespace BugBot.Managers
{
    public static class Card_Importer
    {//itemsPerPage maimum is 36 greater values cause no bugs (that i've seen) but have no effect
        public static async void ImportCards()
        {
            List<Card> allCards = new List<Card>();
            
            Console.WriteLine("Starting Card Import");
            string pageLength = "36";
            string language = "en-en";
            string root = "https://api.altered.gg/cards";
            

            int page = 1;
            int totalItems = -1;
            

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(root);

            while (totalItems != 0)
            {
                string parameters = $"?itemsPerPage={pageLength}&locale={language}&page={page}";
                HttpResponseMessage response = await client.GetAsync(parameters).ConfigureAwait(false);
                Console.WriteLine("Response Recieved");

                if (response.IsSuccessStatusCode)
                {


                    Console.WriteLine("Response Success");

                    //var jsonString = await response.Content.ReadAsStringAsync();
                    //API_Response cards = JsonConvert.DeserializeObject<API_Response>(jsonString);

                    string jsonString = await response.Content.ReadAsStringAsync();

                    JObject temp = JObject.Parse(jsonString);

                    API_Response cards = JsonConvert.DeserializeObject<API_Response>(jsonString);

                    totalItems = cards.getTotalItems();

                    if (cards != null)
                    {
                        foreach (Card c in cards.getMembers())
                        {
                            c.CleanPowersandCosts();
                            Console.WriteLine(c.getName());
                            allCards.Add(c);
                            Trie.Insert(c.getName());
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

            Console.WriteLine($"Import Done, Total number of cards imported is {allCards.Count()}");
            Card_DataBase database = new Card_DataBase(allCards);




           
        }
    }
}
