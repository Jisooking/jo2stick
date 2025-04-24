using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.Scene;
using TextRPG.View;
using TextRPGTemplate.Context;

namespace TextRPGTemplate.Scene
{
    internal class BattleScene_SkillSelect : AScene
    {
        public BattleScene_SkillSelect(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }

        private Skill selectSkill;

        public override void DrawScene()
        {
            ClearScene();
            List<string> dynamicText = new();

            dynamicText.Add("[보유 스킬]");
            dynamicText.Add("");

            for (int i = 0; i < gameContext.ch.learnSkillList.Count; i++)
            {
                if (gameContext.ch.learnSkillList[i].isEquip)
                {
                    selectSkill = gameContext.ch.learnSkillList[i];
                    dynamicText.Add($"- {i + 1} {(selectSkill.isEquip ? "[E]" : "")} {selectSkill.skillName} \t | {(selectSkill.skillType == 0 ? "공격스킬" : "방어스킬")} + {(selectSkill.effectAmount)} 데미지");
                    dynamicText.Add($"\t {selectSkill.description}");
                }
            }

            //dynamicText.Add($"{gameContext.skillList[0].skillName}");

            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            ((SpriteView)viewMap[ViewID.Sprite]).SetText(sceneText.spriteText!);

            Render();
        }


        //기능
        public override string respond(int i)
        {
            UseSkill();
            return SceneID.BattleScene;
        }

        public void UseSkill()
        {
            if (selectSkill.targetType == TargetType.Enemy)
            {
                MonsterData target = ChooseTarget();

                if (target == null) return;

                int skillDamage = (int)((gameContext.ch.getTotalAttack() + selectSkill.effectAmount) + (gameContext.ch.getStat(selectSkill.statType) * selectSkill.skillFactor));

                int damage = (skillDamage - target.Power);
                if (damage < 0) damage = 0;

                target.HP = Math.Max(0, target.HP - damage);
                ((LogView)viewMap[ViewID.Log]).AddLog($"{gameContext.ch.name}가 {target.Name}에게 {selectSkill.skillName}! {damage} 데미지!");

                if (target.HP <= 0)
                {
                    ((LogView)viewMap[ViewID.Log]).AddLog($"{target.Name} 처치!");
                }
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
                if (int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= aliveMonsters.Count)
                {
                    Console.Clear(); // 추가: 화면 정리
                    return aliveMonsters[choice - 1];
                }

                ((InputView)viewMap[ViewID.Input]).SetCursor();
                Console.WriteLine("잘못된 선택입니다. 다시 입력하세요.");
                Console.ReadLine(); // 잘못된 입력 소비
                ((InputView)viewMap[ViewID.Input]).SetCursor();
            }
           ((LogView)viewMap[ViewID.Log]).Update();
            ((LogView)viewMap[ViewID.Log]).Render();
        }
    }
}
