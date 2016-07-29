using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.BitmapFonts;
using System.Collections.Generic;

namespace Pedestrian
{
    public class Scene
    {
        List<IEntity> entities = new List<IEntity>();
        Rectangle borderRectangle = new Rectangle(
            0,
            scoreBoardHeight,
            PedestrianGame.VIRTUAL_WIDTH,
            PedestrianGame.VIRTUAL_HEIGHT - scoreBoardHeight - 1
        );
        Scoreboard scoreboard;
        SpriteBatch spriteBatch;

        const int sidewalkWidth = 16;
        const int scoreBoardHeight = 16;
        const int borderWidth = 1;

        public int Width { get { return PedestrianGame.VIRTUAL_WIDTH; } }
        public int Height { get { return PedestrianGame.VIRTUAL_HEIGHT - scoreBoardHeight - 1; } }

        // Rectangle for play area - area inside border
        public static Rectangle Bounds { get; private set; }
        public static EventEmitter<CoreEvents, IEntity> Events { get; set; }

        public void CreateTombstone(IEntity enemy)
        {
            entities.Add(new Tombstone(enemy.Position));
        }

        public void Load()
        {
            spriteBatch = new SpriteBatch(PedestrianGame.Instance.GraphicsDevice);

            Bounds = new Rectangle(
                borderRectangle.X + borderWidth,
                borderRectangle.Y + borderWidth,
                borderRectangle.Width - 2 * borderWidth,
                borderRectangle.Height - 2 * borderWidth
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
                (int)(sidewalkWidth / 2 + borderWidth),
                (int)(Height * 0.2 + scoreBoardHeight + borderWidth)
            );
            var enemy1 = new Enemy(enemy1Position);

            var enemy2Position = new Vector2(
                (int)(Width - borderWidth - sidewalkWidth / 2),
                (int)(Height * 0.2 + scoreBoardHeight + borderWidth)
            );
            var enemy2 = new Enemy(enemy2Position);

            entities.Add(enemy1);
            entities.Add(enemy2);
            entities.Add(player1);
            entities.Add(player2);

            scoreboard = new Scoreboard(new Player[] { player1, player2 });

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
                borderWidth
            );

            // Space of 2 pixels between each dash in line
            var dashedLineLength = Height - borderWidth - 2;

            var sidewalkLine1Position = new Vector2(sidewalkWidth, scoreBoardHeight + borderWidth);
            var sidewalkLine2Position = new Vector2(Width - sidewalkWidth - borderWidth, scoreBoardHeight + borderWidth);
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
