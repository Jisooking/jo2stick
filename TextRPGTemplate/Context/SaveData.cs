using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPGTemplate.Context;
namespace TextRPG.Context
{
    [Serializable]
    public class SaveData : CharacterBase
    {

        public List<AfterJobStat>? afterJobStat {  get; set; }
        public Item[] items { get; set; } = Array.Empty<Item>();
        public Item[] shopItems { get; set; } = Array.Empty<Item>();
        public Skill[] skillList { get; set; }
        public List<Skill> characterSkillList { get; set; }

        public SaveData() { }

        public SaveData(GameContext gameContext)
        {
            var ch = gameContext.ch;

            name = ch.name;
            job = ch.job;

            Level = ch.Level;
            Str = ch.Str;
            Dex = ch.Dex;
            Int = ch.Int;
            Luk = ch.Luk;

            attack = ch.defaultAttack;
            guard = ch.defaultGuard;
            hp = ch.hp;
            MaxHp = ch.MaxHp;
            Mp = ch.Mp;
            MaxMp = ch.MaxMp;
            Exp = ch.Exp;
            Point = ch.Point;
            CurrentExp = ch.CurrentExp;
            gold = ch.gold;

            critical = ch.critical;
            clearCount = ch.clearCount;

            items = ch.inventory.items!.ToArray();
            shopItems = gameContext.shop.items!.ToArray();
            afterJobStat = gameContext.afterJobStat;

            skillList = gameContext.skillList.ToArray();
            characterSkillList = ch.characterSkillList.ToList();
        }
    }
}
