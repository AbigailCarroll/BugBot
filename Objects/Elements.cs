using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BugBot.Objects
{
    [Serializable]
    class Elements
    {
        [JsonProperty("MAIN_COST")] private string? mainCostRAW;

        [JsonProperty("RECALL_COST")] private string? recallCostRAW;

        [JsonProperty("OCEAN_POWER")] private string? oceanPowerRAW;

        [JsonProperty("MOUNTAIN_POWER")] private string? mountainPowerRAW;

        [JsonProperty("FOREST_POWER")] private string? forestPowerRAW; //need the raw variables to convert to int after json deserialisation
        //couldn't figure out a way to do this faster than how Naurra bot did it. if you have a method that has better space and time compleity dm me.
        [JsonProperty("MAIN_EFFECT")] private string? mainEffect;

        [JsonProperty("ECHO_EFFECT")] private string? echoEffect;

        [JsonProperty("PERMANENT")] private int? permanentSlots;

        [JsonProperty("RESERVE")] private int? reserveSlots;
        private int? forestPower;

        private int? mountainPower;

        private int? oceanPower;

        private int? mainCost;

        private int? recallCost;

        public void CleanPowersandCosts()
        {
            forestPower = RemoveHashes(forestPowerRAW);
            mountainPower = RemoveHashes(mountainPowerRAW);
            oceanPower = RemoveHashes(oceanPowerRAW);
            mainCost = RemoveHashes(mainCostRAW);
            recallCost = RemoveHashes(recallCostRAW);
        }

        private int? RemoveHashes(string? s)
        {
            if (s == null || s.Length == 0) return null;

            string cleanString = Regex.Replace(s, "[^0-9]", "");

            if (int.TryParse(cleanString, out int output))
            {
                return output;
            }
            else
            {
                throw new InvalidCastException("ERR: Could not parse string " + s + " to int, did the API change?");
            }
        }

        public string getCosts()
        {
            return mainCost + "/" + recallCost;
        }

        public string getPowers()
        {
            if (forestPower == null)
            {
                return null;
            }
            return forestPower + "/" + mountainPower + "/" + oceanPower;
        }

        public int? getReserveSlots()
        {
            return reserveSlots;
        }

        public int? getLandmarkSlots()
        {
            return permanentSlots;
        }

        public string getEffects()
        {
            string output = "";
            if (mainEffect != null)
            {
                output += "Main Effect: " + mainEffect;
            }
            if (echoEffect != null)
            {
                output += "\nEcho Effect: " + echoEffect; 
            }
            return output;
        }

    }
}
