using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPGTemplate.Context;

namespace TextRPG.Context
{
    [Serializable]
    public class Character : CharacterBase
    {
        public Inventory inventory { get; set; }
        public List<Skill> characterSkillList { get; set; } = new List<Skill>();
        public List<Skill>? learnSkillList { get; set; }

        public int useableSlot = 5;

        public Character(SaveData saveData) : base()
        {
            // 기본 정보 설정
            name = saveData.name;
            job = saveData.job;
            Level = saveData.Level;
            gold = saveData.gold;
            clearCount = saveData.clearCount;

            Str = saveData.Str;
            Dex = saveData.Dex;
            Int = saveData.Int;
            Luk = saveData.Luk;
            // 스탯 설정

            // 전투 속성 설정
            defaultAttack = saveData.attack;
            defaultGuard = saveData.guard;
            hp = saveData.hp;
            MaxHp = saveData.MaxHp;
            Mp = saveData.Mp;
            MaxMp = saveData.MaxMp;
            Exp = saveData.Exp;
            Point = saveData.Point;
            CurrentExp = saveData.CurrentExp;
            critical = saveData.critical;

            inventory = new Inventory(new List<Item>(saveData.items));
            this.learnSkillList = new List<Skill>(saveData.learnSkillList ?? new List<Skill>());
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

        public void AddJobStat(AfterJobStat afterjobstat)
        {
            Str += (int)(afterjobstat.addStr??0);
            Int += (int)(afterjobstat.addInt??0);
            Dex += (int)(afterjobstat.addDex ?? 0);
            Luk += (int)(afterjobstat.addLuk ?? 0);
            attack += (int)(afterjobstat.addattack ?? 0);
            guard += (int)(afterjobstat.addguard ?? 0);
            hp += (int)(afterjobstat.addHp ?? 0);
            MaxHp += (int)(afterjobstat.addHp ?? 0);
            Mp += (int)(afterjobstat.addMp ?? 0);
            MaxMp += (int)(afterjobstat.addMp ?? 0);
            Point += (int)(afterjobstat.addPoint ?? 0);
            critical += (int)(afterjobstat.addcritical ?? 0);
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