using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.Scene;
using TextRPG.View;
using TextRPGTemplate.Context;

namespace TextRPGTemplate.Scene
{
    internal class SkillScene : AScene
    {
        public SkillScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }

        public override void DrawScene()
        {
            ClearScene();
            List<string> dynamicText = new();

            dynamicText.Add("[보유 스킬]");
            dynamicText.Add("");
            for (int i = 0; i < gameContext.ch.skills.Count; i++)
            {
                Skill tmp = gameContext.ch.skills[i];
                dynamicText.Add($"- {i + 1} {(tmp.isEquip ? "[E]" : "")} {tmp.skillName} \t | {(tmp.skillType == 0 ? "공격스킬" : "방어스킬")} + {(tmp.effectAmount)} 데미지");
                dynamicText.Add($"\t {tmp.description}");
            }

                ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            ((SpriteView)viewMap[ViewID.Sprite]).SetText(sceneText.spriteText!);

            Render();
        }


        //기능
        public override string respond(int i)
        {
            /*
            if(i)
            {
                
            }
            */

            return sceneNext.next![i];
        }
    }
}
