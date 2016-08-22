using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pedestrian.Engine;
using Pedestrian.Engine.Effects;
using Pedestrian.Engine.Graphics;
using Pedestrian.Engine.Input;
using System;

namespace Pedestrian
{
    /// <summary>
    /// This is the main type of the game where the run loop begins.
    /// </summary>
    public class PedestrianGame : Game
    {
        public static PedestrianGame Instance { get; private set; }

        #if DEBUG
        public const bool DEBUG = false;
        #else
        public const bool DEBUG = false;
        #endif

        public const int VIRTUAL_WIDTH = 480;
        public const int VIRTUAL_HEIGHT = 360;
        public const float PREFERRED_ASPECT_RATIO = VIRTUAL_WIDTH / (float)VIRTUAL_HEIGHT;

        public int NumPlayers { get; set; } = 1;
        public GameState CurrentState { get; set; } = GameState.Menu;
        public EventEmitter<GameEvents, IEntity> Events { get; set; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D virtualSizeRenderTarget, fullSizeRenderTarget1, fullSizeRenderTarget2;
        Rectangle destinationRectangle;
        MainMenu menu;
        Scene scene;
        Bloom bloomEffect;
        ScanLines scanLinesEffect;


        public PedestrianGame()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);

            // run fullscreen and fill player's screen resolution
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            if (!DEBUG)
            {
                graphics.ToggleFullScreen();
            }

            Content.RootDirectory = "Content";

            // Set target update and draw rate to 30fps
            TargetElapsedTime = TimeSpan.FromMilliseconds(1 / 30d * 1000);

            Events = new EventEmitter<GameEvents, IEntity>(new CoreEventsComparer());
            Events.AddObserver(GameEvents.GameStart, (e) =>
            {
                CurrentState = GameState.Playing;
                scene = new Scene(NumPlayers);
            });
            Events.AddObserver(GameEvents.GameOver, (e) =>
            {
                CurrentState = GameState.GameOver;
            });
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            Window.ClientSizeChanged += (s, e) => SetDestinationRectangle();
            Window.AllowUserResizing = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Initialize ColorPixels instance
            new PixelTextures(GraphicsDevice);

            var presentationParams = GraphicsDevice.PresentationParameters;

            // Create render target at virtual resolution for game to render to first,
            // then render result upscaled to second render target at full resolution
            // with PointClamp to keep pixel alignment and apply post processing
            virtualSizeRenderTarget = new RenderTarget2D(
                GraphicsDevice,
                VIRTUAL_WIDTH,
                VIRTUAL_HEIGHT);
            fullSizeRenderTarget1 = new RenderTarget2D(
                GraphicsDevice,
                presentationParams.BackBufferWidth,
                presentationParams.BackBufferHeight);
            fullSizeRenderTarget2 = new RenderTarget2D(
                GraphicsDevice,
                presentationParams.BackBufferWidth,
                presentationParams.BackBufferHeight);

            bloomEffect = new Bloom(GraphicsDevice)
            {
                Settings = new BloomSettings("Custom1", 0, 1.2f, 0.9f, 1, 1, 1)
            };
            bloomEffect.LoadContent();
            scanLinesEffect = new ScanLines(GraphicsDevice);
            scanLinesEffect.LoadContent();

            menu = new MainMenu(new Rectangle(0, 0, VIRTUAL_WIDTH, VIRTUAL_HEIGHT));
            menu.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
            virtualSizeRenderTarget.Dispose();
            fullSizeRenderTarget1.Dispose();
            bloomEffect.Unload();
            PixelTextures.Instance.Unload();
            DashedLine.Unload();
        }

        /// <summary>
        /// The update loop that assumes timestep has been fixed
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                GlobalInput.Update();
                if (GlobalInput.WasCommandEntered(InputCommand.Quit))
                {
                    Exit();
                }
                Timers.Update(gameTime);

                if (CurrentState == GameState.Menu)
                {
                    menu.Update(gameTime);
                }
                else
                {
                    scene.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(virtualSizeRenderTarget);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone
            );

            if (CurrentState == GameState.Menu)
            {
                menu.Draw(spriteBatch);
            }
            else if (CurrentState == GameState.Playing || CurrentState == GameState.GameOver)
            {
                scene.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(fullSizeRenderTarget1);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.Opaque,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone
            );
            spriteBatch.Draw(virtualSizeRenderTarget, destinationRectangle, Color.White);
            spriteBatch.End();

            scanLinesEffect.Process(fullSizeRenderTarget1, fullSizeRenderTarget2);
            bloomEffect.Process(fullSizeRenderTarget2, null);
        }

        protected override void EndDraw()
        {
            base.EndDraw();
        }

        private void SetDestinationRectangle()
        {
            var pp = GraphicsDevice.PresentationParameters;
            var screenWidth = pp.BackBufferWidth;
            var screenHeight = pp.BackBufferHeight;

            float outputAspectRatio = (float)screenWidth / screenHeight;

            if (outputAspectRatio <= PREFERRED_ASPECT_RATIO)
            {
                // letterbox - bars on top and bottom
                int gameHeight = (int)(screenWidth / PREFERRED_ASPECT_RATIO + 0.5f);
                int barHeight = (screenHeight - gameHeight) / 2;
                destinationRectangle = new Rectangle(0, barHeight, screenWidth, gameHeight);
            }
            else
            {
                // pillarbox - bars on left and right
                int gameWidth = (int)(screenHeight * PREFERRED_ASPECT_RATIO + 0.5f);
                int barWidth = (screenWidth - gameWidth) / 2;
                destinationRectangle = new Rectangle(barWidth, 0, gameWidth, screenHeight);
            }
        }
    }
}
