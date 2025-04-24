using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;

// QuestClear시 여기로 이동
namespace TextRPG.Scene
{
    public class QuestClearScene : AScene
    {
        public QuestClearScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }

        public override void DrawScene()
        {
            ClearScene();

            List<string> dynamicText = new();
            var quest = gameContext.questData[gameContext.questinput];
            dynamicText.Add($"완료한 퀘스트 : [{quest.npc}]의 부탁으로 [{quest.questitem}] 가져오기");
            dynamicText.Add($"{quest.dropitemcount}/{quest.questfigure}\n");

            quest.dropitemcount -= quest.questfigure;

            dynamicText.Add("보상 : ");
            switch (gameContext.questinput)
            {
                case 1:
                    {
                        gameContext.ch.inventory?.items?.Add(gameContext.shop.items[6]);
                        dynamicText.Add($"상점 주인에게 아이템을 받았습니다!");
                        dynamicText.Add($"받은 아이템 : {gameContext.shop.items[6]}");
                        break;
                    }
                case 2:
                    {
                        gameContext.ch.attack += 2;
                        dynamicText.Add($"술집 아저씨가 수고했다며 가문의 비기를 전수해 줍니다.");
                        dynamicText.Add($"힘이 증가했습니다! ( +2 )");
                        break;
                    }
                case 3:
                    {
                        gameContext.ch.Point += 3;
                        dynamicText.Add($"꼬마가 감사하다며 신비한 약초를 내밉니다.");
                        dynamicText.Add("포인트를 얻었습니다. ( +3 )");
                        break;
                    }
            }
            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            //((SpriteView)viewMap[ViewID.Sprite]).SetText(sceneText.spriteText!);
            Render();
        }

        public override string respond(int i)
        {
            convertSceneAnimationPlay(sceneNext.next![i]);
            return sceneNext.next![i];
        }
    }
}
