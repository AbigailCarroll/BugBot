using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBot.Objects
{
    [Serializable]
    internal class Card_Individual
    {
        [JsonProperty("cardRulings")] private List<Ruling> rulings { get; set; }

        public List<Ruling> getRulings()
        {
            return rulings;
        }

    }
}
