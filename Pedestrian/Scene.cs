using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.BitmapFonts;
using System.Collections.Generic;

namespace Pedestrian
{
    public class Scene
    {
        public const int SCOREBOARD_HEIGHT = 32;
        
        public static EventEmitter<CoreEvents, IEntity> Events { get; set; }

        int Width { get { return PedestrianGame.VIRTUAL_WIDTH; } }
        int Height { get { return PedestrianGame.VIRTUAL_HEIGHT - SCOREBOARD_HEIGHT - 1; } }

        List<IEntity> entities = new List<IEntity>();
        Scoreboard scoreboard;
        GameArea gameArea;
        SpriteBatch spriteBatch;

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
            gameArea = new GameArea(borderRectangle);

            // Initialize the players
            var player1Position = new Vector2((int)(Width * 0.25), (int)(Height * 0.8));
            var player1 = new Player(player1Position);

            var player2Position = new Vector2((int)(Width * 0.75), (int)(Height * 0.8));
            var player2 = new Player(player2Position)
            {
                Color = Color.DimGray,
                Input = new KeyboardInput(KeyboardInput.INPUT_MAP_SECONDARY)
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
            entities.Add(enemy1);
            entities.Add(enemy2);
            entities.Add(player1);
            entities.Add(player2);
            
            // Scoreboard displays player scores and time remaining in the game
            var scoreboardArea = new Rectangle(Point.Zero, new Point(Width, SCOREBOARD_HEIGHT));
            scoreboard = new Scoreboard(new Player[] { player1, player2 }, scoreboardArea);
            scoreboard.Margin = gameArea.SidewalkWidth + gameArea.BorderWidth;
            scoreboard.IsActive = true;

            Events = new EventEmitter<CoreEvents, IEntity>(new CoreEventsComparer());
            Events.AddObserver(CoreEvents.EnemyKilled, CreateTombstone);
        }

        public void Update(GameTime gameTime)
        {
            entities.ForEach(e => e.Update(gameTime));
            Collision.Update(entities);
            UpdateAfterCollision(gameTime);
        }

        public void UpdateAfterCollision(GameTime gameTime)
        {
            scoreboard.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

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
