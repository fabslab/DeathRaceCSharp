using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.BitmapFonts;
using System.Collections.Generic;

namespace Pedestrian
{
    public class Scene
    {
        public const int SIDEWALK_WIDTH = 32;
        public const int SCOREBOARD_HEIGHT = 32;
        public const int BORDER_WIDTH = 2;

        // Rectangle for play area - area inside border
        public static Rectangle Bounds { get; private set; }
        public static EventEmitter<CoreEvents, IEntity> Events { get; set; }

        int Width { get { return PedestrianGame.VIRTUAL_WIDTH; } }
        int Height { get { return PedestrianGame.VIRTUAL_HEIGHT - SCOREBOARD_HEIGHT - 1; } }

        List<IEntity> entities = new List<IEntity>();
        Rectangle borderRectangle;
        Scoreboard scoreboard;
        SpriteBatch spriteBatch;

        public void CreateTombstone(IEntity enemy)
        {
            entities.Add(new Tombstone(enemy.Position));
        }

        public void Load()
        {
            spriteBatch = new SpriteBatch(PedestrianGame.Instance.GraphicsDevice);

            borderRectangle = new Rectangle(
                0,
                SCOREBOARD_HEIGHT,
                Width,
                Height
            );

            Bounds = new Rectangle(
                borderRectangle.X + BORDER_WIDTH,
                borderRectangle.Y + BORDER_WIDTH,
                borderRectangle.Width - 2 * BORDER_WIDTH,
                borderRectangle.Height - 2 * BORDER_WIDTH
            );

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
                (int)(SIDEWALK_WIDTH / 2 + BORDER_WIDTH),
                (int)(Height * 0.2 + SCOREBOARD_HEIGHT + BORDER_WIDTH)
            );
            var enemy1 = new Enemy(enemy1Position);

            var enemy2Position = new Vector2(
                (int)(Width - BORDER_WIDTH - SIDEWALK_WIDTH / 2),
                (int)(Height * 0.2 + SCOREBOARD_HEIGHT + BORDER_WIDTH)
            );
            var enemy2 = new Enemy(enemy2Position);

            entities.Add(enemy1);
            entities.Add(enemy2);
            entities.Add(player1);
            entities.Add(player2);

            scoreboard = new Scoreboard(new Player[] { player1, player2 });
            scoreboard.IsActive = true;

            Events = new EventEmitter<CoreEvents, IEntity>(new CoreEventsComparer());
            Events.AddObserver(CoreEvents.EnemyKilled, CreateTombstone);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in entities)
            {
                entity.Update(gameTime);
            }

            Collision.Update(entities);
            scoreboard.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            Border.Draw(
                spriteBatch,
                borderRectangle,
                Color.White,
                BORDER_WIDTH
            );

            // Space of 2 pixels between each dash in line
            var dashedLineLength = Height - BORDER_WIDTH;

            var sidewalkLine1Position = new Vector2(SIDEWALK_WIDTH, SCOREBOARD_HEIGHT + BORDER_WIDTH);
            var sidewalkLine2Position = new Vector2(Width - SIDEWALK_WIDTH - BORDER_WIDTH, SCOREBOARD_HEIGHT + BORDER_WIDTH);
            DashedLine.Draw(spriteBatch, sidewalkLine1Position, dashedLineLength, Color.White);
            DashedLine.Draw(spriteBatch, sidewalkLine2Position, dashedLineLength, Color.White);

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
