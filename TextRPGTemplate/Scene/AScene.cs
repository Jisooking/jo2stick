using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Context;
using TextRPG.View;
using TextRPGTemplate.Animation;

namespace TextRPG.Scene
{
    public static class SceneID
    {
        public const string Main = "Main";
        public const string Status = "Status";
        public const string Inventory = "Inventory";
        public const string Wear = "Wear";
        public const string Shop = "Shop";
        public const string Nothing = "None";
        public const string Buy = "Buy";
        public const string Rest = "Rest";
        public const string Sell = "Sell";
        public const string DungeonSelect = "DungeonSelect";
        public const string BattleScene = "BattleScene";
        public const string BattleScene_Skill = "BattleScene_Skill";
        public const string DungeonClear = "DungeonClear";
        public const string DungeonFail = "DungeonFail";
        public const string StatUp = "StatUp";
        public const string NPCScene = "NPCScene";
        public const string QuestScene = "QuestScene";
        public const string QuestClearScene = "QuestClearScene";
        public const string GetJob = "GetJob";
        public const string SkillManager = "SkillManager";
        public const string SkillLearn = "SkillLearn";
        public const string SkillEquip = "SkillEquip";

    }
    public abstract class AScene
    {
        protected GameContext gameContext { get; set; }
        protected Dictionary<string, AView> viewMap { get; set; }
        protected SceneText sceneText { get; set; }
        protected SceneNext sceneNext { get; set; }
        Dictionary<int, Func<GameContext, Animation[]>> makeIdleAnimations = new Dictionary<int, Func<GameContext, Animation[]>>();
        Dictionary<int, Func<GameContext, Animation[]>> makeAttackAnimations = new Dictionary<int, Func<GameContext, Animation[]>>();
        public AScene(GameContext gameContext, Dictionary<string, AView> viewMap, SceneText sceneText, SceneNext sceneNext)
        {
            this.gameContext = gameContext;
            this.viewMap = viewMap;
            this.sceneText = sceneText;
            this.sceneNext = sceneNext;
        }
        public void Render()
        {
            foreach (var pair in viewMap)
            {
                pair.Value.Update();
                pair.Value.Render();
            }
            ((InputView)viewMap[ViewID.Input]).SetCursor();
        }
        public virtual void DrawScene()
        {
            ((ScriptView)viewMap[ViewID.Script]).SetText(sceneText.scriptText!);
            ((ChoiceView)viewMap[ViewID.Choice]).SetText(sceneText.choiceText!);
            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(System.Array.Empty<string>());
            foreach (var pair in viewMap)
            {
                pair.Value.Update();
                pair.Value.Render();
            }
            ((InputView)viewMap[ViewID.Input]).SetCursor();
        }
        public abstract string respond(int i);
        public void ClearScene()
        {
            ((ScriptView)viewMap[ViewID.Script]).SetText(sceneText.scriptText!);
            ((ChoiceView)viewMap[ViewID.Choice]).SetText(sceneText.choiceText!);
            ((DynamicView)viewMap[ViewID.Dynamic]).SetText(System.Array.Empty<string>());
            //((SpriteView)viewMap[ViewID.Sprite]).SetText(System.Array.Empty<string>());

            foreach (var pair in viewMap)
            {
                pair.Value.Update();
                pair.Value.Render();
            }
            ((InputView)viewMap[ViewID.Input]).SetCursor();
        }
        public void convertSceneAnimationPlay(int i)
        {
            if (gameContext.animationMap.ContainsKey(sceneNext.next![i]))
            {
                Animation?[] animations = { gameContext.animationMap[sceneNext.next![i]] };
                gameContext.animationPlayer.play(animations, (SpriteView)viewMap[ViewID.Sprite]);
            }
        }

        public void convertSceneAnimationPlay(string s)
        {
            if (gameContext.animationMap.ContainsKey(s))
            {
                Animation?[] animations = { gameContext.animationMap[s] };
                gameContext.animationPlayer.play(animations, (SpriteView)viewMap[ViewID.Sprite]);

            }
        }

        public void battleIdleAnimationPlay()
        {
            List<Animation> animationsList = new List<Animation>();
            Animation[] animationsArray = animationsList.ToArray();
            BattleAnimationPos battleAnimationPos = gameContext.battleAnimationPos[gameContext.currentBattleMonsters.Count];

            Animation animation = gameContext.animationMap["FighterIdle"]!.DeepCopy();
            animation.x[0] += battleAnimationPos.characterPosX;
            animation.y[0] += battleAnimationPos.characterPosY;
            animationsList.Add(animation);

            Debug.WriteLine(" ");
            Debug.WriteLine(gameContext.currentBattleMonsters.Count);

            for (int i = 0; i < gameContext.currentBattleMonsters.Count; i++)
            {
                if (gameContext.currentBattleMonsters[i].HP > 0)
                {
                    animation = gameContext.animationMap["FighterIdle"]!.DeepCopy();
                    animation.x[0] += battleAnimationPos.monsterPosX[i];
                    animation.y[0] += battleAnimationPos.monsterPosY[i];
                    animationsList.Add(animation);
                }
            }
            animationsArray = animationsList.ToArray();

            gameContext.animationPlayer.play(animationsArray, (SpriteView)viewMap[ViewID.Sprite]);
        }

        public void battleAttackAnimationPlay()
        {
            List<Animation> animationsList = new List<Animation>();
            Animation[] animationsArray = animationsList.ToArray();
            gameContext.animationPlayer.play(animationsArray, (SpriteView)viewMap[ViewID.Sprite]);
        }
    }
}