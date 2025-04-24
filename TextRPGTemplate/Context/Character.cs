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
            Str += (int)afterjobstat.addStr!;
            Int += (int)afterjobstat.addInt!;
            Dex += (int)afterjobstat.addDex!;
            Luk += (int)afterjobstat.addLuk!;
            attack += (int)afterjobstat.addattack!;
            guard += (int)afterjobstat.addguard!;
            hp += (int)afterjobstat.addHp!;
            MaxHp += (int)afterjobstat.addHp!;
            Mp += (int)afterjobstat.addMp!;
            MaxMp += (int)afterjobstat.addMp!;
            Point += (int)afterjobstat.addPoint!;
            critical += (int)afterjobstat.addcritical!;
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