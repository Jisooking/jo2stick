// Monster.cs
[Serializable]
public class MonsterData
{
    public string Name { get; set; }
    public int HP { get; set; }
    public int MaxHP { get; set; }  // 최대 체력 속성 추가
    public int Level { get; set; }
    public int Power { get; set; }
    public int ExpReward { get; set; }
    public int GoldReward { get; set; }

    public MonsterData()
    {
        MaxHP = HP;  // JSON에서 로드 시 HP 값으로 MaxHP 초기화
    }
    public MonsterData Clone()
    {
        return new MonsterData
        {
            Name = this.Name,
            HP = this.MaxHP, // 복제 시 항상 풀체력
            Power = this.Power,
            MaxHP = this.MaxHP,
            ExpReward = this.ExpReward,
            GoldReward = this.GoldReward,
        };
    }
}