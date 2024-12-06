using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBot.Objects
{
    [Serializable]
    class Generic_Field
    {
        [JsonProperty("@type")] private string type { get; set; }
        [JsonProperty("reference")] private string reference { get; set; }
        [JsonProperty("name")] private string name { get; set; }
    }
}
