using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Tiling;
using Microsoft.Xna.Framework.Audio;
using Tiler;

namespace AnimatedSprite
{
    public class Projectile : RotatingSprite
    {

        public enum PROJECTILE_STATE { STILL, FIRING, EXPOLODING };
        PROJECTILE_STATE projectileState = PROJECTILE_STATE.STILL;
        protected Game myGame;
        protected float RocketVelocity = 4.0f;
        Vector2 textureCenter;
        Vector2 Target;
        AnimateSheetSprite explosion;
        float ExplosionTimer = 0;
        float ExplosionVisibleLimit = 1000;
        Vector2 StartPosition;
        SoundEffect explosionSound;
        SoundEffectInstance soundEffectInstance;
        string owner;
        bool hasHitTarget = false;
           

            public PROJECTILE_STATE ProjectileState
            {
                get { return projectileState; }
                set { projectileState = value; }
            }

            public AnimateSheetSprite Explosion
            {
                get { return explosion; }
                set { explosion = value; }
            }

            public Projectile(Game g, Vector2 userPosition, List<TileRef> sheetRefs,int frameWidth,int frameHeight,int layerDepth,SoundEffect explosion,string own) 
                : base(g,userPosition,sheetRefs,frameWidth,frameHeight,layerDepth)
            {
                Target = Vector2.Zero;
                myGame = g;
            //textureCenter = new Vector2(texture.Width/2,texture.Height/2);
            //explosion =  rocketExplosion;
            //FrameWidth = frameWidth / 2;
           
                StartPosition = userPosition;
                ProjectileState = PROJECTILE_STATE.STILL;
                explosionSound = explosion;
                soundEffectInstance = explosionSound.CreateInstance();
                owner = own;

        }
        public Projectile(Game g, Vector2 userPosition, List<TileRef> sheetRefs, int frameWidth, int frameHeight, int layerDepth, SoundEffect explosion, string own,Vector2 TargetPosition)
                : base(g, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            Target = Vector2.Zero;
            myGame = g;
            //textureCenter = new Vector2(texture.Width/2,texture.Height/2);
            //explosion =  rocketExplosion;
            //FrameWidth = frameWidth / 2;

            StartPosition = userPosition;
            ProjectileState = PROJECTILE_STATE.STILL;
            explosionSound = explosion;
            soundEffectInstance = explosionSound.CreateInstance();
            owner = own;

        }

        public void AddExplosion()
        {
            Explosion = new AnimateSheetSprite(Game, this.PixelPosition, new List<TileRef>()
                {
                new TileRef(0, 0, 0),
                new TileRef(1, 0, 0),
                new TileRef(2, 0, 0),              
                }, 64, 64, 0f);

            explosion.PixelPosition -= textureCenter;
            explosion.Visible = false;
            explosion.DrawOrder = 2;
        }
            public override void Update(GameTime gametime)
            {
                switch (projectileState)
                {
                    case PROJECTILE_STATE.STILL:
                        this.Visible = false;
                        explosion.Visible = false;
                        break;
                    // Using Lerp here could use target - pos and normalise for direction and then apply
                    // Velocity
                    case PROJECTILE_STATE.FIRING:
                        this.Visible = true;                       
                        PixelPosition = Vector2.Lerp(PixelPosition, Target, 0.02f * RocketVelocity);
                         // rotate towards the Target
                        this.angleOfRotation = TurnToFace(PixelPosition,
                                                Target, angleOfRotation, 1f);
                    if (Vector2.Distance(PixelPosition, Target) < 2)
                        projectileState = PROJECTILE_STATE.EXPOLODING;
                        break;
                    case PROJECTILE_STATE.EXPOLODING:
                   
                        explosion.PixelPosition = Target;
                        explosion.Visible = true;

                        break;
                }
            // if the explosion is visible then just play the animation and count the timer
            if (explosion.Visible)
            {
                explosion.Update(gametime);
                ExplosionTimer += gametime.ElapsedGameTime.Milliseconds;
                
                if (soundEffectInstance.State != SoundState.Playing)
                {
                    soundEffectInstance.Play();
                }
            }
                // if the timer goes off the explosion is finished
                if (ExplosionTimer > ExplosionVisibleLimit)
                {
                    explosion.Visible = false;
                    ExplosionTimer = 0;
                    projectileState = PROJECTILE_STATE.STILL;
                }

                base.Update(gametime);
            }
            public void fire(Vector2 SiteTarget)
            {
                projectileState = PROJECTILE_STATE.FIRING;
                Target = SiteTarget;
            }   
            public override void Draw(GameTime gameTime)
            {
                
                base.Draw(gameTime);
                //spriteBatch.Begin();
                //spriteBatch.Draw(spriteImage, position, SourceRectangle,Color.White);
                //spriteBatch.End();
                if (explosion.Visible)
                    explosion.Draw( gameTime);
                

            }

        public void CheckCollision(TilePlayer player)
        {
           
            if (this.collisionDetect(player)&& hasHitTarget == false)
            {
                player.Health -= 10;
                ProjectileState = PROJECTILE_STATE.EXPOLODING;
                this.Visible = false;
                    
                UpdateHitStatus();

            }
            

            
           

            
        }
        public void CheckCollision(EnemyTank enemy)
        {
            if (this.collisionDetect(enemy) && hasHitTarget == false)
            {

                enemy.Health -= 100;
                ProjectileState = PROJECTILE_STATE.EXPOLODING;
                this.Visible = false;

                UpdateHitStatus();
            }

        }
        public void UpdateHitStatus()
        {
            hasHitTarget = true;
        }

    }
}
