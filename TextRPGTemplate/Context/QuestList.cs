using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TextRPGTemplate.Context
{
    internal class QuestList
    {
        public string request { get; set; }
        public int figure { get; set; }

        public string Questname {  get; set; }
        public int Questfigure {  get; set; }

        public QuestList(string questname, int questfigure)
        {
            Questname = questname;
            Questfigure = questfigure;
        }

    }
}
