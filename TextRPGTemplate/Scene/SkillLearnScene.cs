using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.Scene;
using TextRPG.View;
using TextRPGTemplate.Context;

namespace TextRPG.Scene
{
    public class SkillLearnScene : AScene
    {
        public SkillLearnScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }

        public override void DrawScene()
        {
            ClearScene();
            List<string> dynamicText = new();

            dynamicText.Add("[스킬 목록]");
            dynamicText.Add("");

            for (int i = 0; i < gameContext.skillList.Length; i++)
            {
                Skill learnSkill = gameContext.ch.learnSkillList[i];
                string skillType = "";
                switch (learnSkill.skillType)
                {
                    case SkillType.Attack: skillType = "공격스킬"; break;
                    case SkillType.Defence: skillType = "방어스킬"; break;
                    case SkillType.Utility: skillType = "보조스킬"; break;
                }

                string statType = "";
                switch (learnSkill.statType)
                {
                    case StatType.None: statType = "없음"; break;
                    case StatType.Str: statType = "힘"; break;
                    case StatType.Dex: statType = "민첩"; break;
                    case StatType.Int: statType = "지능"; break;
                    case StatType.Luk: statType = "운"; break;
                }

                dynamicText.Add($"{i + 1}.{(learnSkill.isEquip ? "[E]" : "")} {learnSkill.skillName} | {skillType} | {statType} | 마나 : {learnSkill.costMana} | 횟수 : {learnSkill.maxUseCount} |");
                dynamicText.Add($"\t쿨타임 : {learnSkill.coolTime}턴 | {(learnSkill.duration == 0 ? "즉발" : $"{learnSkill.duration}턴")} | {learnSkill.description} |");
            }

            Render();
        }


        //기능
        public override string respond(int i)
        {
            convertSceneAnimationPlay(sceneNext.next![i]);
            return sceneNext.next![i];
        }
    }
}
