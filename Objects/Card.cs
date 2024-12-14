using BugBot.Managers;
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
        private string cleanName { get; set; }
        [JsonProperty("elements")] private Elements elements { get; set; }


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
            this.cleanName = Input_Manager.Clean_Input(this.name);
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

    }





}
