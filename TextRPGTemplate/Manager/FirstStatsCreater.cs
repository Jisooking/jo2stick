using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TextRPG.Context;

namespace TextRPGTemplate.Managers
{
    public class FirstStatsCreater
    {
        public Random rnd = new Random();
        public int Level = 1;
        public int Str;
        public int Int;
        public int Dex;
        public int Luk;
        public float attack;
        public float guard;
        public string name;
        public string job;
        public int Hp;
        public int MaxHp;
        public int Mp;
        public int MaxMp;
        public int Exp;
        public int Point;
        public int CurrentExp;
        public int MaxExp;
        public int Gold;
        public int BaseExpIncrement;
        public int statLimit = 200;

        public void ShuffleStats()
        {
            int[] stats = { Str, Int, Dex, Luk };
            for (int i = 0; i < stats.Length; i++)
            {
                int j = rnd.Next(i, stats.Length);
                int temp = stats[i];
                stats[i] = stats[j];
                stats[j] = temp;
            }

            Str = stats[0];
            Int = stats[1];
            Dex = stats[2];
            Luk = stats[3];
        }

        private void RandomStats()
        {
            int remaining = statLimit;
            Str = rnd.Next(0, remaining + 1);
            remaining -= Str;
            Int = rnd.Next(0, remaining + 1);
            remaining -= Int;
            Dex = rnd.Next(0, remaining + 1);
            remaining -= Dex;
            Luk = rnd.Next(0, remaining + 1);
            remaining -= Luk;

            ShuffleStats();
        }

        public FirstStatsCreater(bool autoGenerate = true)
        {
            Level = 1;
            CurrentExp = 0;
            MaxExp = 100;
            Point = 0;
            Gold = 20000;
            BaseExpIncrement = 10;
            name = "Hero";  // 기본 이름 설정
            job = "Warrior";  // 기본 직업 설정

            if (autoGenerate)
            {
                RandomStats();
                Calculate();
            }
        }

        public void Calculate()
        {
            MaxHp = 50 + (Str * 2);
            MaxMp = 50 + (Int * 1);

            if (Hp > MaxHp) Hp = MaxHp;
            if (Mp > MaxMp) Mp = MaxMp;

            if (Hp == 0) Hp = MaxHp;
            if (Mp == 0) Mp = MaxMp;

            attack = Str * 1.5f;  // 공격력 계산
            guard = Dex * 1.2f;   // 방어력 계산
        }

        public SaveData ToSaveData()
        {
            SaveData saveData = new SaveData
            {
                Level = Level,
                name = name,
                job = job,
                Str = Str,
                Dex = Dex,
                Int = Int,
                Luk = Luk,
                attack = attack,
                guard = guard,
                hp = Hp,
                MaxHp = MaxHp,
                Mp = Mp,
                MaxMp = MaxMp,
                Exp = Exp,
                Point = 0,
                CurrentExp = 0,
                MaxExp = 100,
                gold = Gold,
                critical = 10, // 예시로 크리티컬 확률 설정
                clearCount = 0,
                items = new Item[0],
                shopItems = new Item[0]
            };

            return saveData;
        }
        public void FirstStats()
        {


            Console.Clear();

            Calculate();
            Hp = MaxHp;
            Mp = MaxMp;

            Console.WriteLine("=== 랜덤 캐릭터 스탯 생성 ===");
            Console.WriteLine($"레벨(Level): {Level}");
            Console.WriteLine($"힘(Str): {Str}");
            Console.WriteLine($"지능(Int): {Int}");
            Console.WriteLine($"민첩(Dex): {Dex}");
            Console.WriteLine($"운(Luk): {Luk}");
            Console.WriteLine($"총합: {Str + Int + Dex}/{statLimit}");
            Console.WriteLine($"체력(Hp): {Hp}/{MaxHp}");
            Console.WriteLine($"마나(Mp): {Mp}/{MaxMp}");
            Console.WriteLine($"골드(Gold): {Gold}");
        }
        public void GenerateStats(bool showWindow = true) // 저기 나타낸 스텟값에서 아래 나타낼 창
        {
            while (true)
            {
                RandomStats();
                Calculate();

                if (showWindow)
                {
                    Console.Clear();
                    FirstStats();
                }

                Console.WriteLine("다시 생성하려면 Enter, 확정하시하려면 Q 입력");
                string? input = Console.ReadLine();

                if (input != null && input.ToLower() == "q")
                {
                    break;  // "Q" 입력시 루프 종료
                }

                // "Enter" 입력시 루프 계속
                else if (input == "")
                {
                    // 아무 것도 하지 않고 루프가 계속됩니다
                }
            }
        }
    }
}