using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;

namespace TextRPG.Scene
{
    internal class StatusScene : AScene
    {
        public StatusScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext) : base(gameContext, viewMap, sceneText, sceneNext)
        {

        }
        public override void DrawScene()
        {
            ClearScene();

            List<string> dynamicText = new();

            Character ch = gameContext.ch;

            dynamicText.Add($" Lv. {ch.Level}\n");
            dynamicText.Add($" 이 름 : {ch.name}\n");
            dynamicText.Add($" 직 업 : {ch.job}\n");
            dynamicText.Add($" 힘 : {ch.Str}\n");
            dynamicText.Add($" 지 능 : {ch.Int}\n");
            dynamicText.Add($" 민 첩 : {ch.Dex}\n");
            dynamicText.Add($" 운 : {ch.Luk}\n");
            dynamicText.Add($"체 력 : {ch.hp} / {ch.MaxHp}");
            dynamicText.Add($"마 나 : {ch.Mp} / {ch.MaxMp}");
            dynamicText.Add($"경험치 : {ch.CurrentExp} / {ch.MaxExp}");
            dynamicText.Add($"Gold : {ch.gold}G");
            dynamicText.Add($"Critical : {ch.critical}");

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
