using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPGTemplate.Animation;

namespace TextRPG.Context
{
    [Serializable]
    public class GameContext
    {
        public Character ch { get; set; }
        public Shop shop { get; set; }
        public List<DungeonData> dungeonList { get; set; } = new List<DungeonData>();
        public List<MonsterData> currentBattleMonsters { get; set; } = new List<MonsterData>();
        public List<MonsterData> monsterList { get; set; } = new List<MonsterData>();
        public List<MonsterData>? clearedMonsters { get; set; }
        public DungeonData? enteredDungeon { get; set; } = null;
        public int prevHp { get; set; }
        public int curHp {  get; set; }
        public int prevGold { get; set; }
        public int curGold {  get; set; }
        public AnimationPlayer animationPlayer { get; set; }

        public void ResetBattleMonsters()
        {
            currentBattleMonsters.Clear();
        }
        public GameContext(SaveData saveData, List<DungeonData> dungeonData, List<MonsterData> monsters, AnimationPlayer animationPlayer)
        {
            ch = new(saveData);
            shop = new(new List<Item>(saveData.shopItems));
            this.dungeonList = new List<DungeonData>(dungeonData);
            this.animationPlayer = animationPlayer;
            this.monsterList = new List<MonsterData>(monsters);
            currentBattleMonsters = new List<MonsterData>();

        }
    }
}
