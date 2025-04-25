using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.Scene;
using TextRPG.View;
using TextRPGTemplate.Context;
using static System.Net.Mime.MediaTypeNames;

namespace TextRPGTemplate.Scene
{
    internal class BattleScene_SkillSelect : AScene
    {
        public BattleScene_SkillSelect(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }

       
        public override void DrawScene()
        {
            ClearScene();
            List<string> dynamicText = new();

            dynamicText.Add("[보유 스킬]");
            dynamicText.Add("");

            Skill equipSKill;
            for (int i = 0; i < gameContext.ch.equipSkillList.Length; i++)
            {
                if (gameContext.ch.equipSkillList[i] != null)
                {
                    equipSKill = gameContext.ch.equipSkillList[i];
                    int skillDamage = (int)((gameContext.ch.getTotalAttack() + equipSKill.effectAmount[0]) + (gameContext.ch.getStat(equipSKill.statType) * equipSKill.skillFactor));

                    dynamicText.Add($"[{i + 1}] {equipSKill.skillName} | {(skillDamage)} 데미지 | 소모 마나 : {equipSKill.costMana}");
                    dynamicText.Add($"    쿨타임 : {equipSKill.curCoolTime}/{equipSKill.coolTime} | 횟수 : {equipSKill.curUseCount}/{equipSKill.maxUseCount}");
                }
            }

            //dynamicText.Add($"{gameContext.skillList[0].skillName}");

            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            Render();
        }


        //기능
        public override string respond(int i)
        {
            if (i > 0 && i <= gameContext.ch.equipSkillList.Length)
            {
                if (gameContext.ch.equipSkillList != null)
                {
                    UseSkill(gameContext.ch.equipSkillList[i]);
                }
                else
                {
                    ((LogView)viewMap[ViewID.Log]).AddLog($"잘못된 입력입니다.");
                    return SceneID.Nothing;
                }
            }
            else if(i < 0 || i > gameContext.ch.equipSkillList.Length)
            {
                ((LogView)viewMap[ViewID.Log]).AddLog($"잘못된 입력입니다.");
                return SceneID.Nothing;
            }
            return SceneID.BattleScene;
        }
        
        public bool IsUseable(Skill selectSkill)
        {
            if (selectSkill.costMana > gameContext.ch.Mp) return false;
            else if(selectSkill.curUseCount == 0) return false;
            else if(selectSkill.curCoolTime < selectSkill.coolTime) return false;
            else return true;
        }

        public void UseSkill(Skill selectSkill)
        {
            if (!IsUseable(selectSkill))
            {
                
            }

            if (selectSkill.targetType == TargetType.Enemy)
            {
                MonsterData target = ChooseTarget();

                if (target == null) return;

                int skillDamage = (int)((gameContext.ch.getTotalAttack() + selectSkill.effectAmount[0]) + (gameContext.ch.getStat(selectSkill.statType) * selectSkill.skillFactor));

                int damage = (skillDamage - target.Power);
                if (damage < 0) damage = 0;

                target.HP = Math.Max(0, target.HP - damage);
                ((LogView)viewMap[ViewID.Log]).AddLog($"{gameContext.ch.name}가 {target.Name}에게 {selectSkill.skillName}! {damage} 데미지!");

                if (target.HP <= 0)
                {
                    ((LogView)viewMap[ViewID.Log]).AddLog($"{target.Name} 처치!");
                }
                else
                {
                    for (int i = 0; i < selectSkill.secondaryEffects.Count; i++)
                    {
                        if (selectSkill.secondaryEffects[i] != SecondaryEffect.None)
                        {
                            target.StatusEffects.Add(new StatusEffect(ConvertEffect(selectSkill.secondaryEffects[i]), selectSkill.duration[i], selectSkill.effectAmount[i]));
                        }
                        else if (selectSkill.secondaryEffects[i] == SecondaryEffect.Pierce)
                        {

                        }
                        else if (selectSkill.secondaryEffects[i] == SecondaryEffect.Overflow)
                        {

                        }
                    }
                }
            }
        }
        
        public StatusEffectType ConvertEffect(SecondaryEffect secondaryEffect)
        {
            StatusEffectType type;
            switch (secondaryEffect)
            {
                case SecondaryEffect.Stun:
                    type = StatusEffectType.Stun;
                    break;
                case SecondaryEffect.DoT:
                    type = StatusEffectType.DoT;
                    break;
                case SecondaryEffect.Curse:
                    type = StatusEffectType.Curse;
                    break;
                default:
                    type = StatusEffectType.None;
                    break;
            }
            return type;
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
