using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextRPGTemplate.Context
{
    [Serializable]

    public  class QuestData
    {

        [JsonPropertyName("key")]
        public string key { get; set; }
        [JsonPropertyName("questitem")]
        public string questitem {  get; set; }
        [JsonPropertyName("questfigure")]
        public int questfigure {  get; set; }
        [JsonPropertyName("npc")]
        public string npc { get; set; }
        [JsonPropertyName("text")]
        public string text { get; set; }
        [JsonPropertyName("acceptquest")]
        public bool acceptquest { get; set; }
        [JsonPropertyName("clearquest")]
        public bool clearquest { get; set; }
        [JsonPropertyName("dropitemcount")]
        public int dropitemcount { get; set; }

        public QuestData(string key, string questitem, int questfigure, string npc, string text, bool acceptquest, bool clearquest, int dropitemcount)
        {
            this.key = key;
            this.questitem = questitem;
            this.questfigure = questfigure;
            this.npc = npc;
            this.text = text;
            this.acceptquest = acceptquest;
            this.clearquest = clearquest;
            this.dropitemcount = dropitemcount;
        }

        public QuestData()
        {

        }

    }
}
