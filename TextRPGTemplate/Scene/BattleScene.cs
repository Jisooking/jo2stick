using System;
using System.Collections.Generic;
using System.Linq;
using TextRPG.Context;
using TextRPG.View;

namespace TextRPG.Scene
{
    internal class BattleScene : AScene
    {
        private Random rnd = new Random();
        private List<MonsterData> monsters;
        private Character player;

        public BattleScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext, List<MonsterData> monsters)
            : base(gameContext, viewMap, sceneText, sceneNext)
        {
            this.monsters = monsters ?? throw new ArgumentNullException(nameof(monsters));
            this.player = gameContext.ch ?? throw new ArgumentNullException(nameof(gameContext.ch));
        }

        public override void DrawScene()
        {
            ClearScene();
            List<string> dynamicText = new();

            foreach (var monster in monsters)
            {
                dynamicText.Add($"몬스터: {monster.Name} | HP: {monster.HP}/{monster.MaxHP}");
            }

            dynamicText.Add($"\n플레이어: {player.name} | HP: {player.hp}/{player.MaxHp} | MP: {player.Mp}/{player.MaxMp}");

            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            ((SpriteView)viewMap[ViewID.Sprite]).SetText(sceneText.spriteText!);
            Render();
        }

        public override string respond(int input)
        {
            // 전투 로직 처리
            if (player.hp <= 0)
            {
                ((LogView)viewMap[ViewID.Log]).AddLog("플레이어는 죽었습니다.");
                return SceneID.DungeonFail;
            }

            // 공격, 마법, 포션 사용 등 전투 행동 처리
            switch (input)
            {
                case 1: // 물리 공격
                    PerformPhysicalAttack();
                    break;

                case 2: // 마법 공격
                    PerformMagicAttack();
                    break;

                case 3: // 도망가기
                    if (TryEscape())
                    {
                        return SceneID.DungeonSelect; // 던전 선택으로 돌아가기
                    }
                    break;

                case 4: // 포션 사용
                    UsePotion();
                    break;

                default:
                    return SceneID.BattleScene; // 다시 전투 화면
            }

            // 몬스터 턴 처리
            foreach (var monster in monsters)
            {
                if (monster.HP > 0)
                {
                    MonsterAttack(monster);
                }
            }

            // 모든 몬스터가 죽으면 던전 클리어
            if (monsters.All(m => m.HP <= 0))
            {
                return SceneID.DungeonClear;
            }

            return SceneID.BattleScene;
        }

        private void PerformPhysicalAttack()
        {
            var target = ChooseTarget();
            if (target == null) return;

            int damage = (int)(player.getTotalAttack() - target.Power);
            if (damage < 0) damage = 0;

            target.HP -= damage;
            ((LogView)viewMap[ViewID.Log]).AddLog($"{player.name}가 {target.Name}에게 물리 공격! {damage} 데미지!");

            if (target.HP <= 0)
            {
                ((LogView)viewMap[ViewID.Log]).AddLog($"{target.Name} 처치!");
            }
        }

        private void PerformMagicAttack()
        {
            var target = ChooseTarget();
            if (target == null) return;

            int damage = (int)(player.getTotalAttack() * 1.5 - target.Power); // 마법은 물리 공격보다 강하게 설정
            if (damage < 0) damage = 0;

            target.HP -= damage;
            ((LogView)viewMap[ViewID.Log]).AddLog($"{player.name}가 {target.Name}에게 마법 공격! {damage} 데미지!");

            if (target.HP <= 0)
            {
                ((LogView)viewMap[ViewID.Log]).AddLog($"{target.Name} 처치!");
            }
        }

        private bool TryEscape()
        {
            int escapeChance = rnd.Next(100);
            if (escapeChance < 50)
            {
                ((LogView)viewMap[ViewID.Log]).AddLog("도망 실패!");
                return false;
            }
            else
            {
                ((LogView)viewMap[ViewID.Log]).AddLog("도망 성공!");
                return true;
            }
        }

        private void UsePotion()
        {
            
        }

        private void MonsterAttack(MonsterData monster)
        {
            if (monster.HP <= 0) return;

            int damage = (int)(monster.Power - player.getTotalGuard());
            if (damage < 0) damage = 0;

            player.hp -= damage;
            ((LogView)viewMap[ViewID.Log]).AddLog($"{monster.Name}가 {player.name}에게 공격! {damage} 데미지!");

            if (player.hp <= 0)
            {
                ((LogView)viewMap[ViewID.Log]).AddLog("플레이어가 쓰러졌습니다. 게임 오버!");
            }
        }

        private MonsterData? ChooseTarget()
        {
            Console.WriteLine("\n어떤 몬스터를 공격하시겠습니까?");
            for (int i = 0; i < monsters.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {monsters[i].Name} (HP: {monsters[i].HP}/{monsters[i].MaxHP})");
            }

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= monsters.Count)
            {
                return monsters[choice - 1];
            }

            Console.WriteLine("잘못된 선택입니다.");
            return null;
        }
    }
}
