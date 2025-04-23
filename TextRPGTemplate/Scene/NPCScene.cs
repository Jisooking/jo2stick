using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;

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
            dynamicText.Add("[마을 사람들과 대화할 수 있습니다.\n");
            dynamicText.Add(" > 1.상점 주인");
            dynamicText.Add("> 2. 술집 아저씨");
            dynamicText.Add("> 3. 꽃을 파는 꼬마");
            dynamicText.Add(">>");
            string input = Console.ReadLine();
            int intinput = int.Parse(input);

            switch (intinput)
            {
                case 1:
                    {
                        break;
                    }
            }

            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());
            //((SpriteView)viewMap[ViewID.Sprite]).SetText(sceneText.spriteText!);
            Render();
        }

        public override string respond(int i)
        {
            return sceneNext.next![i];
        }
    }
}
