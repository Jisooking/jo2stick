using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;

namespace TextRPGTemplate.Context
{
    internal class SkillExecutor
    {
        public Skill registSkill;

        public SkillExecutor(Skill skill) 
        { 
            registSkill = skill;
        }
       
        public virtual void UseSkill()
        {
            //Console.WriteLine($"{registSkill.skillName}  사용");
        }
    }
}
