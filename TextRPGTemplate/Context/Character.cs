using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
