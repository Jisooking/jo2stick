using System;
using System.Collections.Generic;
using System.Linq;
using TextRPG.Context;
using TextRPG.View;
using TextRPGTemplate.Context;

namespace TextRPG.Scene
{
    public class BattleScene : AScene
    {
        private Random rnd = new Random();
        private Character player;

        public BattleScene(GameContext gameContext, Dictionary<string, AView> viewMap,
                 SceneText sceneText, SceneNext sceneNext)
    :   base(gameContext, viewMap, sceneText, sceneNext)
        {

            // 2. 플레이어 참조
            this.player = gameContext.ch ?? throw new ArgumentNullException(nameof(gameContext.ch));

            // 3. 몬스터 리스트 검증
            if (this.gameContext.currentBattleMonsters!.Count == 0)
            {
                throw new InvalidOperationException(
                    $"던전 선택 후 몬스터가 생성되지 않았습니다. " +
                    $"DungeonSelectScene.respond()에서 몬스터를 생성해야 합니다.");
            }
        }

        public override void DrawScene()
        {
            ClearScene();
            List<string> dynamicText = new();
            foreach (var monster in gameContext.currentBattleMonsters!)
            {
                dynamicText.Add($"몬스터: {monster.Name} | HP: {monster.HP}/{monster.MaxHP}");
            }

            dynamicText.Add($"플레이어: {player.name} | HP: {player.hp}/{player.MaxHp} | MP: {player.Mp}/{player.MaxMp}");

            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            //((SpriteView)viewMap[ViewID.Sprite]).SetText(sceneText.spriteText!);
            Render();
        }
        private string? CheckBattleEnd()
        {
            // 모든 몬스터가 죽은 경우
            if (gameContext.currentBattleMonsters!.All(m => m.HP <= 0))
            {
                // 보상 계산 (죽은 몬스터만)
                var deadMonsters = gameContext.currentBattleMonsters!.Where(m => m.HP <= 0).ToList();

                gameContext.clearedMonsters = deadMonsters
                    .Select(m => new MonsterData
                    {
                        Name = m.Name,
                        ExpReward = m.ExpReward,
                        GoldReward = m.GoldReward
                    })
                    .ToList();


                // 전투 몬스터 리스트 초기화

                ((LogView)viewMap[ViewID.Log]).AddLog("모든 몬스터를 처치했습니다!");
                ((LogView)viewMap[ViewID.Log]).ClearText();

                return SceneID.DungeonClear;
            }

            // 플레이어 사망 경우
            if (player.hp <= 0)
            {
                return SceneID.DungeonFail;
            }

            return null; // 전투 계속
        }
        public override string respond(int input)
        {
            var battleResult = CheckBattleEnd();
            if (battleResult != null) return battleResult;

            bool actionPerformed = true;

            switch (input)
            {
                case 1: actionPerformed = PerformPhysicalAttack(); break;
                case 2: actionPerformed = PerformMagicAttack(); break;
                case 3: return sceneNext.next![input];
                case 4: if (TryEscape()) return SceneID.DungeonSelect; break;
                case 5: actionPerformed = UsePotion(); break;
                default:
                    ((LogView)viewMap[ViewID.Log]).AddLog("잘못된 입력입니다. 다시 선택해주세요.");
                    Thread.Sleep(1000);
                    return SceneID.BattleScene;
            }

            if (!actionPerformed)
            {
                // 공격 대상 선택 취소 시 턴 유지
                return SceneID.BattleScene;
            }

            battleResult = CheckBattleEnd();
            if (battleResult != null) return battleResult;

            foreach (var monster in gameContext.currentBattleMonsters!.Where(m => m.HP > 0).ToList())
            {
                MonsterAttack(monster);

                battleResult = CheckBattleEnd();
                if (battleResult != null) return battleResult;
            }
            return SceneID.BattleScene;
        }




        private bool PerformPhysicalAttack()
        {
            var target = ChooseTarget();
            if (target == null) return false;

            int damage = (int)(player.getTotalAttack() *player.Str - target.Power);
            if (damage < 0) damage = 0;

            target.HP = Math.Max(0, target.HP - damage);
            ((LogView)viewMap[ViewID.Log]).AddLog($"{player.name}가 {target.Name}에게 물리 공격! {damage} 데미지!");

            if (target.HP <= 0)
            {
                ((LogView)viewMap[ViewID.Log]).AddLog($"{target.Name} 처치!");
                var dropitemList = new List<string>();
                var quest = gameContext.questData[gameContext.questinput];
                ((LogView)viewMap[ViewID.Log]).AddLog($"{target.Dropitem} 을 얻었습니다.");
                if (quest.questitem == target.Dropitem)
                {
                    dropitemList.Add(target.Dropitem);
                    quest.dropitemcount += dropitemList.Count;
                    ((LogView)viewMap[ViewID.Log]).AddLog($"{target.Dropitem} 을 얻었습니다.");
                    if (quest.dropitemcount >= quest.questfigure)
                    {
                        quest.clearquest = true;
                        gameContext.isaccept = false;
                    }
                }
            }

            return true;
        }


        private bool PerformMagicAttack()
        {
            var target = ChooseTarget();
            if (target == null) return false;

            int damage = (int)(player.getTotalAttack() * player.Int - target.Power); // 마법은 물리 공격보다 강하게 설정
            if (damage < 0) damage = 0;

            target.HP = Math.Max(0, target.HP - damage);
            ((LogView)viewMap[ViewID.Log]).AddLog($"{player.name}가 {target.Name}에게 마법 공격! {damage} 데미지!");

            if (target.HP <= 0)
            {
                ((LogView)viewMap[ViewID.Log]).AddLog($"{target.Name} 처치!");
                var dropitemList = new List<string>();
                var quest = gameContext.questData[gameContext.questinput];
                if (quest.questitem == target.Dropitem)
                {
                    dropitemList.Add(target.Dropitem);
                    quest.dropitemcount += dropitemList.Count;
                    ((LogView)viewMap[ViewID.Log]).AddLog($"{target.Dropitem} 을 얻었습니다.");
                    if (quest.dropitemcount >= quest.questfigure)
                    {
                        quest.clearquest = true;
                        gameContext.isaccept = false;
                    }
                }
            }
            return true;
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
                // 도망 성공 시 몬스터 리스트 초기화
                ((LogView)viewMap[ViewID.Log]).AddLog("도망 성공!");
                return true;
            }
        }

        private bool UsePotion()
        {
            var potions = gameContext.shop.items
                .Where(i => (i.key.Contains("Potion") || i.name?.Contains("포션") == true) && i.quantity > 0)
                .ToList();

            if (potions.Count == 0)
            {
                ((LogView)viewMap[ViewID.Log]).AddLog("사용할 수 있는 포션이 없습니다.");
                return false;
            }

            List<string> dynamicText = new();
            dynamicText.Add("사용할 포션을 선택하세요:");
            for (int i = 0; i < potions.Count; i++)
            {
                dynamicText.Add($"{i + 1}. {potions[i].name} ({potions[i].quantity}개) - {potions[i].description}");
            }
            dynamicText.Add("0. 돌아가기");

            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            Render();

            int choice;
            while (true)
            {
                Console.Write("선택: ");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    if (choice == 0) return false;

                    if (choice >= 1 && choice <= potions.Count)
                    {
                        var selectedPotion = potions[choice - 1];
                        int healAmount = 0;

                        if (selectedPotion.key == "healPotion")
                        {
                            healAmount = 20;
                        }
                        else if (selectedPotion.key == "bighealPotion")
                        {
                            healAmount = 50;
                        }

                        int beforeHp = player.hp;
                        player.hp = Math.Min(player.MaxHp, player.hp + healAmount);

                        selectedPotion.quantity--;
                        ((LogView)viewMap[ViewID.Log]).AddLog($"{selectedPotion.name} 사용! HP {player.hp - beforeHp} 회복!");

                        if (selectedPotion.quantity <= 0)
                        {
                            gameContext.shop.items.Remove(selectedPotion);
                        }

                        return true;
                    }
                }

    ((LogView)viewMap[ViewID.Log]).AddLog("잘못된 입력입니다.");
            }

        }

        private void MonsterAttack(MonsterData monster)
        {
            if (monster.HP <= 0) return;

            int damage = (int)(monster.Power - player.getTotalGuard());
            if (damage < 0) damage = 0;

            player.hp -= damage;

            if (player.hp < 0) player.hp = 0;

            ((LogView)viewMap[ViewID.Log]).AddLog($"{monster.Name}가 {player.name}에게 공격! {damage} 데미지!");

            if (player.hp <= 0)
            {
                ((LogView)viewMap[ViewID.Log]).AddLog("플레이어가 쓰러졌습니다. 게임 오버!");
            }
        }

        private MonsterData? ChooseTarget()
        {
            var aliveMonsters = gameContext.currentBattleMonsters!
                .Where(m => m.HP > 0)
                .ToList();

            if (aliveMonsters.Count == 0)
            {

                ((LogView)viewMap[ViewID.Log]).AddLog("공격할 수 있는 몬스터가 없습니다.");
                return null;
            }
            ((LogView)viewMap[ViewID.Log]).AddLog("어떤 몬스터를 공격하시겠습니까?");
            for (int i = 0; i < aliveMonsters.Count; i++)
            {
                ((LogView)viewMap[ViewID.Log]).AddLog($"{i + 1}. {aliveMonsters[i].Name} (HP: {aliveMonsters[i].HP}/{aliveMonsters[i].MaxHP})");
            }
            ((LogView)viewMap[ViewID.Log]).Update();
            ((LogView)viewMap[ViewID.Log]).Render();

            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    if (choice == 0)
                    {
                        Console.Clear(); // 돌아가기 전 화면 정리
                        return null;     // 호출한 쪽에서 판단하게
                    }
                    if (choice > 0 && choice <= aliveMonsters.Count)
                    {
                        Console.Clear();
                        return aliveMonsters[choice - 1];
                    }
                }

                ((InputView)viewMap[ViewID.Input]).SetCursor();
                ((LogView)viewMap[ViewID.Log]).AddLog("잘못된 선택입니다. 다시 입력하세요.");
                Console.ReadLine(); // 잘못된 입력 소비
                ((InputView)viewMap[ViewID.Input]).SetCursor();
            }
            ((LogView)viewMap[ViewID.Log]).Update();
            ((LogView)viewMap[ViewID.Log]).Render();
        }
    }
}
