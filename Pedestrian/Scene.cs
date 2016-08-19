using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine;
using Pedestrian.Engine.Collision;
using System.Collections.Generic;

namespace Pedestrian
{
    public class Scene
    {
        public const int SCOREBOARD_HEIGHT = 30;
        
        public static EventEmitter<GameEvents, IEntity> Events { get; set; }

        int Width { get { return PedestrianGame.VIRTUAL_WIDTH; } }
        int Height { get { return PedestrianGame.VIRTUAL_HEIGHT - SCOREBOARD_HEIGHT - 1; } }

        List<IEntity> entities = new List<IEntity>();
        Scoreboard scoreboard;
        SpriteBatch spriteBatch;
        bool gameOver = false;

        public void CreateTombstone(IEntity enemy)
        {
            entities.Add(new Tombstone(enemy.Position));
        }

        public void Load()
        {
            spriteBatch = new SpriteBatch(PedestrianGame.Instance.GraphicsDevice);

            var borderRectangle = new Rectangle(
                0,
                SCOREBOARD_HEIGHT,
                Width,
                Height
            );
            var gameArea = new PlayArea(borderRectangle, 32, 2);
            var roadArea = new RoadBounds(new Rectangle(
                    gameArea.Bounds.X + gameArea.SidewalkWidth,
                    gameArea.Bounds.Y,
                    gameArea.Bounds.Width - 2 * gameArea.SidewalkWidth,
                    gameArea.Bounds.Height
                )
            );

            // Initialize the players
            var player1Position = new Vector2((int)(Width * 0.25), (int)(Height * 0.8));
            var player1 = new Player(player1Position, PlayerIndex.One);

            var player2Position = new Vector2((int)(Width * 0.75), (int)(Height * 0.8));
            var player2 = new Player(player2Position, PlayerIndex.Two)
            {
                Color = new Color(70, 90, 100)
            };

            // Initialize the enemies - start them in specific positions
            var enemy1Position = new Vector2(
                (int)(gameArea.SidewalkWidth / 2 + gameArea.BorderWidth),
                (int)(Height * 0.2 + SCOREBOARD_HEIGHT + gameArea.BorderWidth)
            );
            var enemy1 = new Enemy(enemy1Position);

            var enemy2Position = new Vector2(
                (int)(Width - gameArea.BorderWidth - gameArea.SidewalkWidth / 2),
                (int)(Height * 0.2 + SCOREBOARD_HEIGHT + gameArea.BorderWidth)
            );
            var enemy2 = new Enemy(enemy2Position);

            entities.Add(gameArea);
            entities.Add(roadArea);
            entities.Add(enemy1);
            entities.Add(enemy2);
            entities.Add(player1);
            entities.Add(player2);

            // Scoreboard displays player scores and time remaining in the game
            var scoreboardArea = new Rectangle(0, 0, Width, SCOREBOARD_HEIGHT);
            scoreboard = new Scoreboard(new Player[] { player1, player2 }, scoreboardArea);
            scoreboard.Margin = gameArea.SidewalkWidth + gameArea.BorderWidth;
            scoreboard.IsActive = true;

            Events = new EventEmitter<GameEvents, IEntity>(new CoreEventsComparer());
            Events.AddObserver(GameEvents.EnemyKilled, CreateTombstone);
            Events.AddObserver(GameEvents.GameOver, e => gameOver = true);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in entities)
            {
                if (entity is Player)
                {
                    if (!gameOver)
                    {
                        entity.Update(gameTime);
                    }
                }
                else
                {
                    entity.Update(gameTime);
                }
            }
            Collision.Update(entities);
            UpdateAfterCollision(gameTime);
        }

        public void UpdateAfterCollision(GameTime gameTime)
        {
            scoreboard.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone
            );

            scoreboard.Draw(gameTime, spriteBatch);

            foreach (var entity in entities)
            {
                entity.Draw(gameTime, spriteBatch);
                if (PedestrianGame.DEBUG)
                {
                    entity.DrawDebug(gameTime, spriteBatch);
                }
            }

            spriteBatch.End();
        }
    }
}
