using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Tiling;
using Tiler;
using Microsoft.Xna.Framework.Audio;

namespace AnimatedSprite
{
    class EnemyTank:RotatingSprite
    {
        private int fireRate = 2500;
        private int remainingReloadTime = 0;
        private float shootingArea = 500;
        private Projectile bullet;
        private SoundEffect shootingSound;
        private SoundEffectInstance soundEffectInstance;
        SoundEffect explosionSound;
        

        private enum FireState { Ready, NotReady}

        private FireState fireState = FireState.Ready;

        public Projectile Bullet
        {
            get
            {
                return bullet;
            }

            set
            {
                bullet = value;
            }
        }

        public EnemyTank(Game game, Vector2 userPosition,List<TileRef>sheetRefs,int frameWidth,int frameHeight,float layerDepth,SoundEffect shoot,SoundEffect explosion ) : base(game,userPosition,sheetRefs, frameWidth,  frameHeight, layerDepth)
        {
            shootingSound = shoot;
            explosionSound = explosion;
            soundEffectInstance = shootingSound.CreateInstance();
        }
        public void loadProjectile(Projectile r)
        {
            Bullet = r;
        }


        public override void Update(GameTime gameTime)
        {
           // arrow.Update(gameTime);

          
           
            base.Update(gameTime);
        }

        public bool FaceThePlayer(TilePlayer p)
        {
            this.angleOfRotation = TurnToFace(PixelPosition, p.PixelPosition, angleOfRotation, 0.2f);
            return true;
        }
        public bool IsPlayerInShootingArea(TilePlayer p)
        {
            float distance = Math.Abs(Vector2.Distance(this.PixelPosition, p.PixelPosition));

            if (distance < shootingArea)
                return true;
            else return false;

        }
    
        public bool IsReadyToFire()
        {
            if (fireState == FireState.Ready) return true;
            else return false;          
            
        }

        public void CreateBullet()
        {
            Bullet = new Projectile(Game, this.PixelPosition, new List<TileRef>()
                {
                new TileRef(8, 0, 0),
                }, FrameWidth, FrameHeight, 1,explosionSound,"Enemy");
            Bullet.AddExplosion();
            Bullet.DrawOrder = 2;

        }
        public void ReloadArrow(GameTime gametime)
        {
            

            if (remainingReloadTime <= 0)
            {
                
                remainingReloadTime = fireRate;
                UpdateFireState(true);

            }else if (remainingReloadTime >= 0)
            {
                remainingReloadTime -= gametime.ElapsedGameTime.Milliseconds;
                UpdateFireState(false);
            }

            
        }

        public void UpdateFireState(bool status)
        {
            if (status) fireState = FireState.Ready;
            else fireState = FireState.NotReady;
        }



        public void FireArrow(TilePlayer p)
        {
            
            Bullet.PixelPosition = this.PixelPosition;
            
            Bullet.fire(p.PixelPosition);
            PlayGunFire();


            
                                                
            UpdateFireState(false);
                      
        }

        public bool IsPlayerHit(TilePlayer p)
        {
            if (Bullet.SourceRectangle.Contains(p.PixelPosition.ToPoint())) return true;
            else return false;
        }

        public override void Draw(GameTime gameTime)
        {
            /*
            if(arrow!= null)
            {
                arrow.Draw(spriteBatch);
            }

    */
            base.Draw(gameTime);
        }

        public void PlayGunFire()
        {
            
            soundEffectInstance.Play();
        }
    }
}
