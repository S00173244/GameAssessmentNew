using AnimatedSprite;
using Engine.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tiler;
using Tiling;

namespace CameraNS
{
    class Camera : GameComponent
    {
        static Vector2 _camPos = Vector2.Zero;
        static Vector2 _worldBound;
        public static Matrix CurrentCameraTranslation { get
            {
                return Matrix.CreateTranslation(new Vector3(
                    -CamPos,
                    0));
            } }
        public float CameraSpeed = 5.0f;
        public static Vector2 CamPos
        {
            get
            {
                return _camPos;
            }

            set
            {
                _camPos = value;
            }
        }

        public Camera(Game game, Vector2 startPos, Vector2 bound) : base(game)
        {
            game.Components.Add(this);
            CamPos = startPos;
            _worldBound = bound;
        }

        public override void Update(GameTime gameTime)
        {
            if (InputEngine.IsKeyHeld(Keys.Left))
                move(new Vector2(-1, 0) * CameraSpeed, Game.GraphicsDevice.Viewport);
            if (InputEngine.IsKeyHeld(Keys.Right))
                move(new Vector2(1, 0) * CameraSpeed, Game.GraphicsDevice.Viewport);
            if (InputEngine.IsKeyHeld(Keys.Down))
                move(new Vector2(0, 1) * CameraSpeed, Game.GraphicsDevice.Viewport);
            if (InputEngine.IsKeyHeld(Keys.Up))
                move(new Vector2(0, -1) * CameraSpeed, Game.GraphicsDevice.Viewport);

            TilePlayer p = (TilePlayer)Game.Services.GetService(typeof(TilePlayer));
            if (p != null)
            {
                follow(p.PixelPosition, Game.GraphicsDevice.Viewport);

                //Make sure the player stays in the bounds
                p.PixelPosition = Vector2.Clamp(p.PixelPosition, Vector2.Zero,
                                                new Vector2(_worldBound.X - p.BoundingRectangle.Width,
                                                            _worldBound.Y - p.BoundingRectangle.Height));

            }
            base.Update(gameTime);
        }

        public void move(Vector2 delta, Viewport v)
        {
            CamPos += delta;
            CamPos = Vector2.Clamp(CamPos, Vector2.Zero, _worldBound - new Vector2(v.Width, v.Height));
        }

        public static void follow(Vector2 followPos, Viewport v)
        {
            _camPos = followPos - new Vector2(v.Width / 2, v.Height / 2);
            _camPos = Vector2.Clamp(_camPos, Vector2.Zero, _worldBound - new Vector2(v.Width, v.Height));
        }

    }
}
