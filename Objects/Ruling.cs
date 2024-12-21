using Discord.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugBot.Objects
{
    [Serializable]
    public class Ruling
    {
        [JsonProperty("question")] private string question { get; set; }

        [JsonProperty("answer")] private string answer { get; set; }

        [JsonProperty("createdAt")] private string createdAt { get; set; }

        [JsonProperty("@id")] private string path { get; set; }

        public string getQuestion()
        {
            return this.question;
        }
        public string getAnswer()
        {
            return this.answer;
        }

        
    }
}
