using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;
using TextRPGTemplate.Context;

// 새 Scene을 만들때 복붙
namespace TextRPG.Scene
{
    public class QuestScene : AScene
    {

        public QuestScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }

        public override void DrawScene()
        {
            ClearScene();

            List<string> dynamicText = new();
            var quest = gameContext.questData[gameContext.questinput];
            if (!quest.acceptquest)
            {
                dynamicText.Add($"{quest.npc} :");
                dynamicText.Add($"{quest.text}\n");
                dynamicText.Add("퀘스트를 수락하시겠습니까?");
            }
            else if (quest.acceptquest)
            {
                dynamicText.Add("진행중인 퀘스트입니다.");
            }

            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            //((SpriteView)viewMap[ViewID.Sprite]).SetText(sceneText.spriteText!);
            Render();
        }

        public override string respond(int i)
        {
            var quest = gameContext.questData[gameContext.questinput];
            if (i == 1)
            {
                quest.acceptquest = true;
                ((LogView)viewMap[ViewID.Log]).AddLog("퀘스트가 수락되었습니다.");
                ((LogView)viewMap[ViewID.Log]).AddLog($"구해야 할 아이템 : {quest.questitem}");
                ((LogView)viewMap[ViewID.Log]).AddLog($"[몬스터 드롭 아이템]/{quest.questfigure}");
            }
            else if (i == 0)
            {
                return sceneNext.next![i];
            }

            convertSceneAnimationPlay(sceneNext.next![i]);
            return sceneNext.next![i];
        }
    }
}
