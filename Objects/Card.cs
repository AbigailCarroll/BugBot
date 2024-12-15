using BugBot.Managers;
using Discord.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BugBot.Objects
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    [Serializable]
    public class Card
    {
        [JsonProperty("reference")] private string reference { get; set; }
        [JsonProperty("cardType")] private Generic_Field cardType { get; set; }
        [JsonProperty("rarity")] private Generic_Field rarity { get; set; }
        [JsonProperty("imagePath")] private string imagePath { get; set; }
        //[JsonProperty("qrUrlDetail")]
        //not necessary to store as the link to the card details page at altered.gg can be constructed using the card's reference.
        [JsonProperty("mainFaction")] private Generic_Field mainFaction { get; set; }
        [JsonProperty("name")] private string name { get; set; }
        private string cleanName { get; set; }
        [JsonProperty("elements")] private Elements elements { get; set; }

        private List<Ruling> rulings { get; set; }
        private List<string> questions { get; set; }
        private List<string> answers { get; set; }

        public string getName() //
        {
            return name;
        }

        public string getCleanName()
        {
            return cleanName;
        }
        public void CleanPowersandCosts()
        {
            elements.CleanPowersandCosts();
        }

        public void CleanName()
        {
            string cardName = this.name;
            

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(cardName);
            cardName = System.Text.Encoding.UTF8.GetString(tempBytes);

            cardName = Regex.Replace(cardName, @"&", "and");
            cardName = Regex.Replace(cardName, @"[^a-zA-Z]", ""); //removes any non alphabetic characters from the card name

            cardName = cardName.ToLower();
            cleanName = cardName;
        }
        public string getReference()
        {
            return reference;
        }

        public string getImagePath()
        {
            return imagePath;
        }

        public string getCosts()
        {
            return elements.getCosts();
        }

        public string getPowers()
        {
            return elements.getPowers();
        }

        public string getCardType()
        {
            return cardType.getName();
        }

        public string getFaction()
        {
            return mainFaction.getName();
        }

        public int? getReserveSlots()
        {
            return elements.getReserveSlots();
        }

        public int? getLandmarkSlots()
        {
            return elements.getLandmarkSlots();
        }

        public string getRarity()
        {
            return rarity.getName();
        }

        public string getEffects()
        {
            return elements.getEffects();
        }
    
        public List<string> getRulings()
        {
            if(rulings == null)
            {
                Console.WriteLine("Rulings was null");
                return null;
            }
            List<string> output = new List<string>();

            foreach (Ruling ruling in rulings)
            {
                output.Add("Q: " + ruling.getQuestion() + "\n\n" + "A: "+ ruling.getAnswer());
            }

            return output;
        }

        public async void setRulings() //altered api only provudes rulings in english so no need for alternate language support (yet)
        {
            string language = "en-en";

            string parameters = "?" + language;
            string root = "https://api.altered.gg/cards/" + reference;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(root);

            HttpResponseMessage response = await client.GetAsync(parameters).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();

                JObject temp = JObject.Parse(jsonString);

                Card_Individual cardRulings = JsonConvert.DeserializeObject<Card_Individual>(jsonString);

                rulings = cardRulings.getRulings();
            }
            else
            {
                Console.WriteLine("Response failed for card " + name + " and reference " + reference);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(root + parameters);

            }
        }

      


    }





}
