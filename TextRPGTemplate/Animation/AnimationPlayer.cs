using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.View;

namespace TextRPGTemplate.Animation
{
    public class AnimationPlayer
    {
        public AnimationPlayer()
        {

        }

        public void play(Animation[] animations, SpriteView spriteView)
        {
            ViewPort viewPort = spriteView.view!;
            int x = viewPort.x;
            int y = viewPort.y;
            

            Thread thread = new(() =>
            {
                for (int i = 0; i < animations.Length; i++)
                {
                    for (int j = 0; j < animations[i].frames.Length; j++)
                    {
                        spriteView.SetText(animations[i].frames);
                        spriteView.Update();
                        spriteView.Render();
                        Thread.Sleep(animations[i].frameDurationMs);
                    }
                }
            });
            thread.IsBackground = true;
            thread.Start();
            thread.Join();
        }
    }
}
