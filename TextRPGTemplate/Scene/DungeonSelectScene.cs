using System;
using System.Collections.Generic;
using System.Linq;
using TextRPG.Context;
using TextRPG.View;

namespace TextRPG.Scene
{
    internal class DungeonSelectScene : AScene
    {
        private Random rnd = new Random();

        public DungeonSelectScene(GameContext gameContext, Dictionary<string, AView> viewMap,
                                SceneText sceneText, SceneNext sceneNext)
            : base(gameContext, viewMap, sceneText, sceneNext) { }

        public override void DrawScene()
        {
            ClearScene();
            List<string> dynamicText = new();

            for (int i = 0; i < gameContext.dungeonList.Count; i++)
            {
                var dungeon = gameContext.dungeonList[i];
                dynamicText.Add($"{i + 1}. {dungeon.Name} \t| 방어력 {dungeon.RecommendedDefense} 이상 권장");
            }

            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            ((SpriteView)viewMap[ViewID.Sprite]).SetText(sceneText.spriteText!);
            Render();
        }

        public override string respond(int i)
        {
            if (i > 0 && i <= gameContext.dungeonList.Count)
            {
                var dungeon = gameContext.dungeonList[i - 1];
                gameContext.enteredDungeon = dungeon;
                gameContext.prevHp = gameContext.ch.hp;
                gameContext.prevGold = gameContext.ch.gold;

                // 던전에 적합한 몬스터 생성
                var dungeonMonsters = GenerateMonstersForDungeon(dungeon);

                if (dungeonMonsters == null || dungeonMonsters.Count == 0)
                {
                    ((LogView)viewMap[ViewID.Log]).AddLog("해당 던전에 적합한 몬스터가 없습니다!");
                    return SceneID.Nothing;
                }

                // 생성된 몬스터를 GameContext에 저장
                gameContext.currentBattleMonsters = dungeonMonsters;
                return SceneID.BattleScene;
            }

            return sceneNext.next![i];
        }

        private List<MonsterData> GenerateMonstersForDungeon(DungeonData dungeon)
        {
            var monsters = new List<MonsterData>();
            int monsterCount = rnd.Next(dungeon.MonsterCountMin, dungeon.MonsterCountMax + 1);

            for (int i = 0; i < monsterCount; i++)
            {
                var monster = GenerateMonsterForDungeon(dungeon);
                if (monster != null)
                {
                    monsters.Add(monster);
                }
            }

            return monsters;
        }

        private MonsterData GenerateMonsterForDungeon(DungeonData dungeon)
        {
            // 던전의 MonsterTypes에 해당하는 몬스터만 필터링
            var validMonsters = gameContext.monsterList
                .Where(m => dungeon.MonsterTypes.Contains(m.Name))
                .ToList();

            if (validMonsters.Count == 0)
            {
                ((LogView)viewMap[ViewID.Log]).AddLog($"경고: {dungeon.Name}에 설정된 몬스터 타입이 없습니다!");
                return null;
            }

            // 가중치 랜덤 선택 (레벨이 높을수록 확률 감소)
            var selected = WeightedRandomSelection(validMonsters);
            return selected?.Clone();
        }

        private MonsterData WeightedRandomSelection(List<MonsterData> monsters)
        {
            // 레벨이 낮을수록 선택 확률 높임 (가중치 = 1/레벨)
            var weights = monsters.Select(m => 1f / m.Level).ToList();
            float totalWeight = weights.Sum();
            float randomValue = (float)rnd.NextDouble() * totalWeight;

            for (int i = 0; i < monsters.Count; i++)
            {
                if (randomValue < weights[i])
                {
                    return monsters[i];
                }
                randomValue -= weights[i];
            }

            return monsters.Last();
        }
    }
}