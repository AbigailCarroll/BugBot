using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using BugBot.Managers.Emotes;
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

        [JsonProperty("FOREST_POWER")] private string? forestPowerRAW; 

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
                output += "\n" + echoEffect; 
            }
            return output;
        }

        public void AddEmotes()
        {
            string pattern = "{.}";
            MatchEvaluator evaluator = new MatchEvaluator(ReplaceCode);


            if (mainEffect != null)
            {
                mainEffect = mainEffect.Replace("[]", "");
                mainEffect = mainEffect.Replace("[", "**");
                mainEffect = mainEffect.Replace("]", "**");
                mainEffect = Regex.Replace(mainEffect, pattern, evaluator);
            }

            if (echoEffect != null)
            {
                echoEffect = echoEffect.Replace("[]", "");
                echoEffect = echoEffect.Replace("[", "**");
                echoEffect = echoEffect.Replace("]", "**");
                echoEffect = Regex.Replace(echoEffect, pattern, evaluator);
            }

        }

        private string ReplaceCode(Match m)
        {         
            return Emote_Manager.GetEmote(m.Value.ToUpper());
        }

    }
}
