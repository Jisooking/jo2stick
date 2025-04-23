using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;
using TextRPGTemplate.Context;

// 새 Scene을 만들때 복붙
namespace TextRPG.Scene
{
    internal class QuestScene : AScene
    {
        public QuestScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
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
            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(dynamicText.ToArray());

            Render();
        }

        public override string respond(int i)
        {
            List<QuestList> quests = new List<QuestList>();
            quests.Add(new QuestList("고블린의 이빨", 5));
            quests.Add(new QuestList("오크의 가죽", 10));
            quests.Add(new QuestList("슬라임의 눈물", 15));

            List<bool> istrue = new List<bool>();
            istrue.Add(false);
            istrue.Add(false);
            istrue.Add(false);


            return sceneNext.next![i];
        }
    }
}
