using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;
using TextRPGTemplate.Context;

// Quest를 받는 씬
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

            for (int j = 0; j <= quests.Count; j++)
            {
                if (intinput > 0 && intinput <= quests.Count)
                {
                    if (intinput == 1 && j == 0)
                    {
                        while (!istrue[j])
                        {

                            Console.WriteLine("\n수락하기 : 1");
                            Console.WriteLine("돌아가기 : 0");
                            Console.Write(">>");

                            string answer = Console.ReadLine();
                            if (answer == "1")
                            {
                                QuestAnswer(j);
                            }
                            else if (answer == "0")
                            {
                                QuestScripts();
                                return;
                            }
                            else
                            {
                                Console.WriteLine("잘못된 입력입니다.");
                                Thread.Sleep(1000);
                                QuestScripts();
                                return;
                            }
                        }
                        if (istrue[j])
                        {
                            Console.WriteLine("이미 퀘스트를 진행 중입니다.");
                            Thread.Sleep(1000);
                            QuestScripts();
                            return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                    QuestScripts();
                    return;
                }
            }
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
        public void QuestAnswer(int j)
        {
            Console.WriteLine("퀘스트를 수락했습니다!");

            acceptquests.Add(new AcceptQuest(quests[j].Questname, quests[j].Questfigure));
            Console.WriteLine($"찾아야 할 것 : {quests[j].Questname} {quests[j].Questfigure}개");

            istrue[j] = true;
            Thread.Sleep(2000);
            QuestScripts();
            return;
        }
        public void NPC1()
        {
            Console.WriteLine("상점 주인 :");
            Console.WriteLine("\"이런, 재료가 다 떨어졌잖아.\"");
            Console.WriteLine("\"용사님, 혹시 [{}]을 [{}]개 정도만 가져다 주실 수 있으신가요?\"");
            Console.WriteLine($"\"가져와 주시면 공격력을 올려주는 포션을 드릴게요!\"");
        }
        public void NPC2()
        {
            Console.WriteLine("술집 아저씨 :");
            Console.WriteLine("\"으하하! 거기 자네! 바쁘지 않으면 [{}]을 [{}]만큼만 가져다 줄 수 있겠는가?\"");
            Console.WriteLine("질 좋은{}가 필요해서 말이야.");
            Console.WriteLine($"\"보상으로는 돈을 섭섭치 않게 챙겨주겠네!\"");
        }
        public void NPC3()
        {
            Console.WriteLine("\"꽃을 파는 꼬마 :\"");
            Console.WriteLine("\"안녕하세요, 용사님! 혹시 [{}]을 [{}]마리 정도만 없애주실 수 있으신가요?\"");
            Console.WriteLine("\"요즘 제가 좋아하는 꽃밭을 망쳐서 아주 골치아프다니까요!\"");
            Console.WriteLine("\"다 물리쳐 주시면 제가 가장 아끼는 꽃 한 송이를 드릴게요...!\"");
        }
    }
}
