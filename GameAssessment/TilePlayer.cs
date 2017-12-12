using AnimatedSprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tiling;

namespace Tiler
{

    public class TilePlayer : RotatingSprite
    {
        //List<TileRef> images = new List<TileRef>() { new TileRef(15, 2, 0)};
        //TileRef currentFrame;
        int speed = 5;
        float turnspeed = 0.03f;
        public Vector2 previousPosition;
        public enum Status {ALIVE,DEAD}
        Status playerStatus = Status.ALIVE;
        private Projectile bullet;
        private SoundEffect shootingSound;
        private SoundEffectInstance soundEffectInstance;
        SoundEffect explosionSound;
        private int fireRate = 1000;
        private int remainingReloadTime = 0;
        private enum FireState { Ready, NotReady }
        public bool objectivesComplete = false;
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

        public Status PlayerStatus
        {
            get
            {
                return playerStatus;
            }

            set
            {
                playerStatus = value;
            }
        }

        public TilePlayer(Game game, Vector2 userPosition, 
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth, SoundEffect shoot, SoundEffect explosion)
                : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            DrawOrder = 1;
            shootingSound = shoot;
            explosionSound = explosion;
            soundEffectInstance = shootingSound.CreateInstance();
            CreateBullet();
        }

        public void Collision(Collider c)
        {
            if (BoundingRectangle.Intersects(c.CollisionField))
                PixelPosition = previousPosition;
        }

        public override void Update(GameTime gameTime)
        {
            //Console.WriteLine(Health);
            UpdatePlayerStatus();
            previousPosition = PixelPosition;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                this.PixelPosition += new Vector2(1, 0) * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this.PixelPosition += new Vector2(-1, 0) * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                this.PixelPosition += new Vector2(0, -1) * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                this.PixelPosition += new Vector2(0, 1) * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                this.angleOfRotation -= turnspeed;
            //Console.WriteLine("PixelPosition : {0}",PixelPosition);
            //Console.WriteLine("Origin : {0}",Origin);
            if (Keyboard.GetState().IsKeyDown(Keys.X))
                this.angleOfRotation += turnspeed;

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (IsReadyToFire())
                {
                    
                    CreateBullet();
                    FireGun();
                    UpdateFireState(false);
                }
                else
                {
                    ReloadArrow(gameTime);
                }
                

                //if(re)
            }
            // Check for collisions

            var colliders = Game.Components.Where(c => c.GetType() == typeof(Collider));
            foreach (var collider in colliders)
            {

            }

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void UpdatePlayerStatus()
        {
            if (Health <= 0)
            {
                PlayerStatus = Status.DEAD;
            }
        }

        public void FireGun()
        {
            Console.WriteLine(Mouse.GetState().X);
            Console.WriteLine(Mouse.GetState().Y);

            
            Bullet.fire(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
        }

        public void CreateBullet()
        {
            Bullet = new Projectile(Game, this.PixelPosition + Origin, new List<TileRef>()
                {
                new TileRef(8, 0, 0),
                }, FrameWidth, FrameHeight, 1, explosionSound, "Player");
            Bullet.AddExplosion();
            Bullet.DrawOrder = 2;

        }

        public void loadProjectile(Projectile r)
        {
            Bullet = r;
        }

        public bool IsReadyToFire()
        {
            if (fireState == FireState.Ready) return true;
            else return false;

        }

        public void ReloadArrow(GameTime gametime)
        {


            if (remainingReloadTime <= 0)
            {

                remainingReloadTime = fireRate;
                UpdateFireState(true);

            }
            else if (remainingReloadTime >= 0)
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


    }
}
