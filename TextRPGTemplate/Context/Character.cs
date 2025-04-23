using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPGTemplate.Context;

namespace TextRPG.Context
{
    [Serializable]
    public class Character
    {
        public int Level { get; set; }
        public string? name { get; set; }
        public string? job { get; set; }
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
        public int MaxExp => (int)(100 * Math.Pow(1.2, Level - 1));
        public int gold { get; set; }

        public float critical { get; set; }

        public float defaultAttack { get; set; } // 현재 레벨 기본 공격력
        public float defaultGuard { get; set; } // 현재 레벨 기본 방어력
        public int clearCount { get; set; }
        public Inventory inventory { get; set; }
        public List<Skill>? learnSkillList { get; set; }

        public Character(SaveData saveData)
        {
            this.name = saveData.name;
            this.job = saveData.job;
            this.defaultAttack = saveData.attack;
            this.defaultGuard = saveData.guard;
            this.hp = saveData.hp;
            this.gold = saveData.gold;
            this.Level = saveData.Level;
            this.name = saveData.name;
            this.job = saveData.job;
            this.Str = saveData.Str;
            this.Int = saveData.Int;
            this.Dex = saveData.Dex;
            this.Luk = saveData.Luk;
            this.defaultAttack = saveData.attack;
            this.defaultGuard = saveData.guard;
            this.hp = saveData.hp;
            this.MaxHp = saveData.MaxHp;
            this.Mp = saveData.Mp;
            this.MaxMp = saveData.MaxMp;
            this.Exp = saveData.Exp;
            this.Point = saveData.Point;
            this.gold = saveData.gold;
            this.CurrentExp = saveData.CurrentExp;
            this.MaxMp = saveData.MaxMp;
            this.clearCount = saveData.clearCount;
            this.inventory = new Inventory(new List<Item>(saveData.items));
            this.critical = saveData.critical;
            try
            {
                learnSkillList = new List<Skill>(saveData.learnSkill);
            }
            catch
            {
                learnSkillList = new List<Skill>();
            }
        }

        public Character(string name, string job, float attack, float guard, int hp, int gold, int clearCount, Inventory inventory, float critical)
        {
            this.name = name;
            this.job = job;
            this.defaultAttack = attack;
            this.defaultGuard = guard;
            this.hp = hp;
            this.gold = gold;
            this.clearCount = clearCount;
            this.inventory = inventory;
            this.critical = critical;
        }

        public int getLevel()
        {  
            while (CurrentExp >= MaxExp)
            {
                CurrentExp -= MaxExp;
                Level++;
                Point += 3;

                Console.WriteLine($"레벨업! 현재 레벨: {Level}, 포인트: {Point}");
                Console.WriteLine($"현재 EXP: {CurrentExp} / {MaxExp}");
            }
            return Level;
        }


        public float getNoWeaponAttack()
        {
            return defaultAttack + (getLevel() - 1) * 0.5f;
        }

        public float getNoArmorGuard()
        {
            return defaultGuard + (getLevel() - 1) * 1.0f;
        }

        public int getPlusAttack()
        {
            int plusAttack = 0;
            foreach (var item in inventory.items!)
            {
                if (item.equiped == true)
                {
                    plusAttack += item.attack;
                }
            }
            return plusAttack;
        }
        public int getPlusGuard()
        {
            int plusGuard = 0;
            foreach (var item in inventory.items!)
            {
                if (item.equiped == true)
                {
                    plusGuard += item.guard;
                }
            }
            return plusGuard;
        }

        public float getTotalAttack()
        {
            return getNoWeaponAttack() + getPlusAttack();
        }

        public float getTotalGuard()
        {
            return getNoArmorGuard() + getPlusGuard();
        }

        public int getStat(StatType stat)
        {
            switch (stat)
            {
                case StatType.None:
                    return 1;
                case StatType.Str:
                    return Str;
                case StatType.Dex:
                    return Dex;
                case StatType.Int:
                    return Int;
                case StatType.Luk:
                    return Luk;
            }

            return 0;
        }
    }
}
