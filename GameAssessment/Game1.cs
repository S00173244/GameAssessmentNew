using AnimatedSprite;
using CameraNS;
using Engine.Engines;
using GameAssessment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using Tiler;
using Tiling;

namespace TileBasedPlayer20172018
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int tileWidth = 64;
        int tileHeight = 64;
        List<TileRef> TileRefs = new List<TileRef>();
        List<Collider> colliders = new List<Collider>();
        string[] backTileNames = { "blue box", "pavement", "blue steel", "green box", "home" };
        List<EnemyTank> enemyList = new List<EnemyTank>();
        CountdownTimer countDownTimer;
        Song backgroundMusic;
        Song defeatMusic;
        Song victoryMusic;
        StartPositionTile startPosition;
        EndPositionTile endPosition;
        public enum GameStatus { PLAYING, VICTORY, DEFEAT }
        GameStatus currentGameStatus = GameStatus.PLAYING;
        
        public enum TileType { BLUEBOX, PAVEMENT, BLUESTEEL, GREENBOX ,HOME };
        int[,] tileMap = new int[,]
    {
        {1,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,2,3,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,1,1,1,1,1,1,1,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2},
        {2,1,1,2,2,2,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,2,0,0,0,0,0,2,0,0,0,0,2,0,0,0,2,0,0,0,2,2,2,2,3,2,2,2,2},
        {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,3,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,2,1,1,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,2,1,1,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,3,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
    };
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            new Camera(this, Vector2.Zero, 
                new Vector2(tileMap.GetLength(1) * tileWidth, tileMap.GetLength(0) * tileHeight));
            new InputEngine(this);

            startPosition = new StartPositionTile(this, new Vector2(64, 128), new List<TileRef>(){new TileRef(10, 1, 0) }, 64, 64, 1f);
            endPosition = new EndPositionTile(this, new Vector2(25*64, 13*64), new List<TileRef>() { new TileRef(0, 2, 0) }, 64, 64, 1f);
            Services.AddService(new TilePlayer(this, startPosition.PixelPosition, new List<TileRef>()
            {
                new TileRef(15, 2, 0),
                new TileRef(15, 3, 0),
                new TileRef(15, 4, 0),
                new TileRef(15, 5, 0),
                new TileRef(15, 6, 0),
                new TileRef(15, 7, 0),
                new TileRef(15, 8, 0),
            }, 64, 64, 1f, Content.Load<SoundEffect>("Sound/gunSound"), Content.Load<SoundEffect>("Sound/explosionSound")));
            SetColliders(TileType.BLUESTEEL);
            SetColliders(TileType.BLUEBOX);

            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(spriteBatch);
            Services.AddService(Content.Load<Texture2D>(@"Tiles/tank tiles 64 x 64"));

            // Tile References to be drawn on the Map corresponding to the entries in the defined 
            // Tile Map
            // "free", "pavement", "ground", "blue", "home" 
            TileRefs.Add(new TileRef(4, 2, 0));
            TileRefs.Add(new TileRef(3, 3, 1));
            TileRefs.Add(new TileRef(6, 3, 2));
            TileRefs.Add(new TileRef(6, 2, 3));
            TileRefs.Add(new TileRef(0, 2, 4));
            // Names fo the Tiles
            
            new SimpleTileLayer(this, backTileNames, tileMap, TileRefs, tileWidth, tileHeight);

            CreateEnemies();

            countDownTimer = new CountdownTimer(50000, Content.Load<SpriteFont>("Font/CountDownFont"), this);
            backgroundMusic = Content.Load<Song>("Music/backgroundMusic");
            victoryMusic = Content.Load<Song>("Music/victory");
            defeatMusic = Content.Load<Song>("Music/defeat");

            //List<Tile> found = SimpleTileLayer.getNamedTiles("green box");
            // TODO: use this.Content to load your game content here
        }

        public void SetColliders(TileType t)
        {
            for (int x = 0; x < tileMap.GetLength(1); x++)
                for (int y = 0; y < tileMap.GetLength(0); y++)
                {
                    if (tileMap[y, x] == (int)t)
                    {
                        colliders.Add(new Collider(this,
                            Content.Load<Texture2D>(@"Tiles/collider"),
                            x, y
                            ));
                    }

                }
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
           


            countDownTimer.UpdateTime(gameTime);

            if (Services.GetService<TilePlayer>().PlayerStatus == TilePlayer.Status.DEAD || countDownTimer.IsTimeLeft() == false)
            {
                GameStatus previousStatus = currentGameStatus;
                currentGameStatus = GameStatus.DEFEAT;
                if (previousStatus != currentGameStatus) MediaPlayer.Stop();
            }
            else if (Services.GetService<TilePlayer>().PlayerStatus == TilePlayer.Status.ALIVE && enemyList.Count <= 0)
            {
                Services.GetService<TilePlayer>().objectivesComplete = true;

                if (Services.GetService<TilePlayer>().collisionDetect(endPosition)){
                    GameStatus previousStatus = currentGameStatus;
                    currentGameStatus = GameStatus.VICTORY;
                    if (previousStatus != currentGameStatus) MediaPlayer.Stop();
                }
                
            }
            else if (Services.GetService<TilePlayer>().PlayerStatus == TilePlayer.Status.ALIVE && enemyList.Count > 0)
            {
                GameStatus previousStatus = currentGameStatus;
                currentGameStatus = GameStatus.PLAYING;
                if (previousStatus != currentGameStatus) MediaPlayer.Stop();
            }
            

            switch (currentGameStatus)
            {
                case GameStatus.PLAYING:
                    if (MediaPlayer.State == MediaState.Stopped)
                    {
                        
                        MediaPlayer.Play(backgroundMusic);
                        
                        
                        
                    }
                    foreach (EnemyTank item in enemyList)
                    {

                        if (item.IsReadyToFire())
                        {
                            item.CreateBullet();

                            if (item.IsPlayerInShootingArea(Services.GetService<TilePlayer>()))
                            {

                                item.FaceThePlayer(Services.GetService<TilePlayer>());
                                item.FireArrow(Services.GetService<TilePlayer>());

                            }
                        }
                        else
                        {
                            item.FaceThePlayer(Services.GetService<TilePlayer>());
                            item.ReloadArrow(gameTime);
                        }
                        
                        

                        item.Update(gameTime);
                    }


                    for (int i = 0; i < enemyList.Count; i++)
                    {
                        Services.GetService<TilePlayer>().Bullet.CheckCollision(enemyList[i]);
                        if (enemyList[i].EnemyStatus == EnemyTank.Status.DEAD) enemyList.Remove(enemyList[i]);
                    }
                        
                        
                    

                    
                    break;

                case GameStatus.DEFEAT:
                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(defeatMusic);
                    }
                    break;

                case GameStatus.VICTORY:
                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(victoryMusic);
                    }
                    break;


            }

            //if(Services.GetService<TilePlayer>().PlayerStatus == TilePlayer.Status.DEAD)
            //{

            //}else if(Services.GetService<TilePlayer>().PlayerStatus == TilePlayer.Status.ALIVE)
            {
                
            }
            
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        public void CreateEnemies()
        {
            
            for (int i = 0; i < 5; i++)
            {
                EnemyTank enemy = new EnemyTank(this, new Vector2(64*(i*i*i), 64*(i)), new List<TileRef>()
                {
                new TileRef(21, 2, 0),
                new TileRef(21, 3, 0),
                new TileRef(21, 4, 0),
                new TileRef(21, 5, 0),
                new TileRef(21, 6, 0),
                new TileRef(21, 7, 0),
                new TileRef(21, 8, 0),
                }, 64, 64, 0f,Content.Load<SoundEffect>("Sound/gunSound"), Content.Load<SoundEffect>("Sound/explosionSound"));

                enemyList.Add(enemy);

            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (EnemyTank item in enemyList)
            {
                
                item.Draw(gameTime);

            }

            startPosition.Draw(gameTime);
            // TODO: Add your drawing code here
            countDownTimer.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
