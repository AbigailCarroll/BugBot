using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BugBot.Objects
{
    [Serializable]
    class API_Response //when the API is called it's first deserialised into this class, if totalItems = 0 then there are no items on that 
    //page and the import is complete.
    {
        [JsonProperty("hydra:totalItems")]
        private int totalItems { get; set; }
        [JsonProperty("hydra:member")]
        private List<Card> members { get; set; }

        public int getTotalItems()
        {
            return totalItems;
        }

        public List<Card> getMembers()
        {
            return members;
        }
    }
}
