using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;
using TextRPGTemplate.Context;

// 마을 사람들과 대화
namespace TextRPG.Scene
{
    public class NPCScene : AScene
    {
        public NPCScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }

        public override void DrawScene()
        {
            ClearScene();

            List<string> dynamicText = new();
            dynamicText.Add("누구와 대화를 나눠보시겠습니까?\n");
            dynamicText.Add("> 1. 상점 주인");
            dynamicText.Add("> 2. 술집 아저씨");
            dynamicText.Add("> 3. 꽃을 파는 꼬마");

            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            //((SpriteView)viewMap[ViewID.Sprite]).SetText(sceneText.spriteText!);
            Render();
        }

        public override string respond(int i)
        {

            if (i == 0)
            {
                return sceneNext.next![i];
            }
            else if (i < gameContext.questData.Length + 1)
            {
                gameContext.questinput = i;
                if (!gameContext.questData[gameContext.questinput].clearquest)
                {
                    return SceneID.QuestScene;
                }
                else if (gameContext.questData[gameContext.questinput].clearquest)
                {
                    return SceneID.QuestClearScene;
                }
            }
            else
            {
                ((LogView)viewMap[ViewID.Log]).AddLog("잘못된 입력입니다.");
                return sceneNext.next![i];
            }
            convertSceneAnimationPlay(sceneNext.next![i]);
            return sceneNext.next![i];
        }
    }
}
