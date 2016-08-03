using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;
        Rectangle destinationRectangle;
        Scene scene;
        double targetUpdateTime = 1 / 30d * 1000;
        double timeSinceUpdate = 0;

        public PedestrianGame()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);

            // run fullscreen and fill player's screen resolution
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //graphics.ToggleFullScreen();

            Content.RootDirectory = "Content";
            TargetElapsedTime = TimeSpan.FromMilliseconds(targetUpdateTime);
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

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Render game to own render target at virtual resolution
            // then render that to back buffer at full resolution
            renderTarget = new RenderTarget2D(GraphicsDevice, VIRTUAL_WIDTH, VIRTUAL_HEIGHT);

            Window.ClientSizeChanged += (s, e) => UpdateDestinationRectangle();
            Window.AllowUserResizing = true;
            Window.Position = new Point();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            scene = new Scene();
            scene.Load();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Ensure update timestep is fixed so that draw rate
            // can be freely changed without affecting gameplay
            timeSinceUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeSinceUpdate >= targetUpdateTime)
            {
                var updateTime = new GameTime(
                    gameTime.TotalGameTime,
                    TimeSpan.FromMilliseconds(timeSinceUpdate),
                    gameTime.IsRunningSlowly
                );
                FixedStepUpdate(updateTime);
                timeSinceUpdate -= targetUpdateTime;
            }
        }

        /// <summary>
        /// The update loop that assumes timestep has been fixed
        /// </summary>
        /// <param name="gameTime"></param>
        protected void FixedStepUpdate(GameTime gameTime)
        {
            if (IsActive)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Exit();
                }
                scene.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);
            scene.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget, destinationRectangle, Color.White);
            spriteBatch.End();
        }

        private void UpdateDestinationRectangle()
        {
            float outputAspectRatio = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
            if (outputAspectRatio <= PREFERRED_ASPECT_RATIO)
            {
                // letterbox with bars on top and bottom
                int presentHeight = (int)(Window.ClientBounds.Width / PREFERRED_ASPECT_RATIO + 0.5f);
                int barHeight = (Window.ClientBounds.Height - presentHeight) / 2;
                destinationRectangle = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
            }
            else
            {
                // pillarbox with bars on left and right
                int presentWidth = (int)(Window.ClientBounds.Height * PREFERRED_ASPECT_RATIO + 0.5f);
                int barWidth = (Window.ClientBounds.Width - presentWidth) / 2;
                destinationRectangle = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
            }
        }
    }
}
