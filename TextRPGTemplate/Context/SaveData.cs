using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TextRPG.Context
{
    [Serializable]
    public class SaveData
    {
        public string? name { get; set; }
        public string? job { get; set; }
        public List<AfterJobStat>? afterJobStat { get; set; }
        public int Level { get; set; }
        public int Str { get; set; }
        public int Dex { get; set; }
        public int Int { get; set; }
        public int Luk { get; set; }

        public float attack { get; set; }
        public float guard { get; set; }
        public int hp { get; set; }
        public int MaxHp { get; set; }
        public int Mp { get; set; }
        public int MaxMp { get; set; }
        public int Exp { get; set; }
        public int Point { get; set; }
        public int CurrentExp { get; set; }
        public int MaxExp { get; set; }
        public int gold { get; set; }

        public float critical { get; set; }
        public int clearCount { get; set; }

        public Item[] items { get; set; } = Array.Empty<Item>();
        public Item[] shopItems { get; set; } = Array.Empty<Item>();

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
            MaxExp = ch.MaxExp;
            gold = ch.gold;

            critical = ch.critical;
            clearCount = ch.clearCount;

            items = ch.inventory.items!.ToArray();
            shopItems = gameContext.shop.items!.ToArray();
            afterJobStat = gameContext.afterJobStat;
        }
    }
}
