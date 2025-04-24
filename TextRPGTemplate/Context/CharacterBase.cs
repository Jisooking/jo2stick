using System;
using System.Collections.Generic;
using TextRPGTemplate.Context;

namespace TextRPG.Context
{
    [Serializable]
    public class CharacterBase
    {
        // 기본 정보
        public string name { get; set; }
        public string job { get; set; }
        public int Level { get; set; }
        public int gold { get; set; }
        public int clearCount { get; set; }

        // 스탯 정보
        public Dictionary<string, int> Stats { get; protected set; }

        // 전투 관련 속성
        public float attack { get; set; }
        public float guard { get; set; }
        public int hp { get; set; }
        public int Str { get; set; }
        public int Int { get; set; }
        public int Dex { get; set; }
        public int Luk { get; set; }
        public int MaxHp { get; set; }
        public int Mp { get; set; }
        public int MaxMp { get; set; }
        public int Exp { get; set; }
        public int CurrentExp { get; set; }
        public int MaxExp => (int)(100 * Math.Pow(1.2, Level - 1));
        public int Point { get; set; }
        public float critical { get; set; }


        // 기본 공격력/방어력
        public float defaultAttack { get; set; }
        public float defaultGuard { get; set; }



        // 스탯 접근을 위한 인덱서
        public int this[string statName]
        {
            get => Stats.TryGetValue(statName, out int value) ? value : 0;
            set
            {
                if (Stats.ContainsKey(statName))
                {
                    Stats[statName] = value;
                }
            }
        }

        // 스탯 추가 메서드
        public void AddStat(string statName, int initialValue = 0)
        {
            if (!Stats.ContainsKey(statName))
            {
                Stats.Add(statName, initialValue);
            }
        }

        // 스탯 합계 계산
        public int GetTotalStats()
        {
            int total = 0;
            foreach (var stat in Stats.Values)
            {
                total += stat;
            }
            return total;
        }
    }
}