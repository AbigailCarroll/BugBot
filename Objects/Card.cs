using Discord.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        [JsonProperty("elements")] private Elements elements { get; set; }


        public string getName()
        {
            return name;
        }
        public void CleanPowersandCosts()
        {
            elements.CleanPowersandCosts();
        }
        public string getReference()
        {
            return reference;
        }
    }





}
