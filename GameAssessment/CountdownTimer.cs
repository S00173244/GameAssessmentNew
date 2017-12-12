using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimatedSprite;
using CameraNS;
using Engine.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Tiler;
using Tiling;

namespace GameAssessment
{
    class CountdownTimer:DrawableGameComponent
    {
        int countDownTime;
        SpriteFont spriteFont;
        Game game;
       
        public CountdownTimer(int countDown, SpriteFont sf,Game g):base(g)
        {
            countDownTime = countDown;
            spriteFont = sf;
            game = g;
            DrawOrder = 2;
         
            
        }
        public void UpdateTime(GameTime g)
        {
            countDownTime -= g.ElapsedGameTime.Milliseconds;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sp = game.Services.GetService<SpriteBatch>();

            //Console.WriteLine("Countdown time:"+countDownTime/1000);
            sp.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend, null, null, null, null, Camera.CurrentCameraTranslation);
            
           // sp.Begin();
            sp.DrawString(spriteFont,countDownTime.ToString(),new Vector2(game.GraphicsDevice.Viewport.Width-200,30), Color.Red,0f,Vector2.Zero,1f,SpriteEffects.None,1f);
            sp.End();

            base.Draw(gameTime);

        }

        public bool IsTimeLeft()
        {
            if (countDownTime > 0) return true;
            else return false;
        }
    }
}
