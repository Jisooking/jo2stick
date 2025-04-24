using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;

namespace TextRPG.Scene
{
    public class ShopScene : AScene
    {
        public ShopScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }
        public override void DrawScene()
        {
            ClearScene();

            List<string> dynamicText = new();
            dynamicText.Add("[보유 골드]");
            dynamicText.Add($"{gameContext.ch.gold} G");
            dynamicText.Add("");
            dynamicText.Add("[아이템 목록]");
            for (int i = 0; i < gameContext.shop?.items?.Count; i++)
            {
                Item tmp = gameContext.shop.items[i];

                // 포션인 경우와 일반 아이템인 경우 구분
                if (tmp.isPotion)
                {
                    // 포션 효과 설명 생성
                    string effect = "";
                    if (tmp.healAmount > 0) effect += $"체력 +{tmp.healAmount} ";
                    if (tmp.manaAmount > 0) effect += $"마나 +{tmp.manaAmount}";

                    dynamicText.Add($"- {tmp.name} | {effect} | {(tmp.bought ? "구매완료" : tmp.price + "G")}");
                }
                else
                {
                    // 일반 아이템: 기존 방식 유지
                    dynamicText.Add($"- {tmp.name} | {(tmp.attack > 0 ? "공격력" : "방어력")} + {(tmp.attack > 0 ? tmp.attack : tmp.guard)} | {(tmp.bought ? "구매완료" : tmp.price + "G")}");
                }

                dynamicText.Add($" {tmp.description}");
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
