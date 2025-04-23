// MonsterList.cs
using System.Text.Json;

[Serializable]
public class MonsterList
{
    public List<MonsterData> monsters { get; set; } = new();

    public void LoadFromJson(string jsonPath)
    {
        string json = File.ReadAllText(jsonPath);
        monsters = JsonSerializer.Deserialize<List<MonsterData>>(json) ?? new();
    }

    public MonsterData? GetMonster(string name)
    {
        return monsters.FirstOrDefault(m => m.Name == name)?.Clone();
    }
}