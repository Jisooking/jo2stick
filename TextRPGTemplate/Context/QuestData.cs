using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TextRPGTemplate.Context
{
    [Serializable]

    public  class QuestData
    {

        public string key { get; set; }
        public string questitem {  get; set; }
        public int questfigure {  get; set; }
        public string npc { get; set; }
        public string text { get; set; }
        public bool acceptquest { get; set; }
        public bool clearquest { get; set; }
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
