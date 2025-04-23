using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;

// 새 Scene을 만들때 복붙
namespace TextRPG.Scene
{
    public class StatUpScene : AScene
    {
        public StatUpScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }

        public override void DrawScene()
        {
            ClearScene();

            List<string> dynamicText = new();
            dynamicText.Add("어떤 스텟을 올릴까요?");
            dynamicText.Add($"1. 공격력 : {gameContext.ch.defaultAttack}");
            dynamicText.Add($"2. 방어력 : {gameContext.ch.defaultGuard}");
            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            //((SpriteView)viewMap[ViewID.Sprite]).SetText(sceneText.spriteText!);

            Render();
        }

        public override string respond(int i)
        {
            switch (i)
            {
                case 1: gameContext.ch.defaultAttack++; break;
                case 2: gameContext.ch.defaultGuard++; break;
            }
            return sceneNext.next![i];
        }
    }
}
