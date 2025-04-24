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
