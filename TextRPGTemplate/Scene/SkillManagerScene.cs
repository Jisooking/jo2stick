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
    public class SkillManagerScene : AScene
    {
        public SkillManagerScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }

        public override void DrawScene()
        {
            ClearScene();
            List<string> dynamicText = new();

            dynamicText.Add("[스킬 목록]");
            dynamicText.Add("");

            if (gameContext.ch.characterSkillList == null)
            {
                dynamicText.Add("현재 사용할 수 있는 스킬이 없습니다.");
            }
            else
            {
                for (int i = 0; i < gameContext.ch.characterSkillList.Count; i++)
                {
                    Skill skill = gameContext.ch.characterSkillList[i];

                    string skillType = "";
                    switch (skill.skillType)
                    {
                        case SkillType.Attack: skillType = "공격스킬"; break;
                        case SkillType.Defence: skillType = "방어스킬"; break;
                        case SkillType.Utility: skillType = "보조스킬"; break;
                    }

                    string statType = "";
                    switch (skill.statType)
                    {
                        case StatType.None: statType = "없음"; break;
                        case StatType.Str: statType = "힘"; break;
                        case StatType.Dex: statType = "민첩"; break;
                        case StatType.Int: statType = "지능"; break;
                        case StatType.Luk: statType = "운"; break;
                    }

                    dynamicText.Add($"{i + 1}.{(skill.isEquip ? "[E]" : "")} {skill.skillName}");
                    dynamicText.Add($"   {skill.description}");
                    dynamicText.Add("");
                }
            }
            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            ((SpriteView)viewMap[ViewID.Sprite]).SetText(sceneText.spriteText!);

            Render();
        }


        //기능
        public override string respond(int i)
        {
            return sceneNext.next![i];
        }
    }
}
