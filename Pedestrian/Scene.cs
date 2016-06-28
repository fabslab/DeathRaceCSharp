using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Pedestrian
{
    public class Scene
    {
        List<IEntity> entities = new List<IEntity>();
        List<PedestrianEnemy> enemies = new List<PedestrianEnemy>();
        List<PlayerCar> players = new List<PlayerCar>();

        SpriteBatch spriteBatch;
        public const int sidewalkWidth = 16;
        public const int scoreBoardHeight = 16;
        public const int borderWidth = 1;

        public int Width { get { return PedestrianGame.VIRTUAL_WIDTH; } }
        public int Height { get { return PedestrianGame.VIRTUAL_HEIGHT; } }
        public static Rectangle Bounds { get; private set; }
            = new Rectangle(
                0,
                scoreBoardHeight + borderWidth,
                PedestrianGame.VIRTUAL_WIDTH,
                PedestrianGame.VIRTUAL_HEIGHT - scoreBoardHeight - borderWidth - 1
              );


        public void Load()
        {
            spriteBatch = new SpriteBatch(PedestrianGame.Instance.GraphicsDevice);

            // Initialize the players
            var player1Position = new Vector2((int)(Width * 0.25), (int)(Height * 0.8));
            var player1 = new PlayerCar(player1Position);

            var player2Position = new Vector2((int)(Width * 0.75), (int)(Height * 0.8));
            var player2 = new PlayerCar(player2Position)
            {
                Color = Color.DimGray,
                Input = new KeyboardInput(KeyboardInput.INPUT_MAP_SECONDARY)
            };

            // Initialize the enemies - start them in specific positions
            var enemy1Position = new Vector2(
                (int)(sidewalkWidth / 2 + borderWidth),
                (int)(Height * 0.2 + scoreBoardHeight + borderWidth)
            );
            var enemy1 = new PedestrianEnemy(enemy1Position);

            var enemy2Position = new Vector2(
                (int)(Width - borderWidth - sidewalkWidth / 2),
                (int)(Height * 0.2 + scoreBoardHeight + borderWidth)
            );
            var enemy2 = new PedestrianEnemy(enemy2Position);

            players.Add(player1);
            players.Add(player2);

            enemies.Add(enemy1);
            enemies.Add(enemy2);

            entities.Add(enemy1);
            entities.Add(enemy2);
            entities.Add(player1);
            entities.Add(player2);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in entities)
            {
                entity.Update(gameTime);
            }
            Collision.Update(players, enemies);
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            Border.Draw(
                spriteBatch,
                Bounds,
                Color.White,
                borderWidth
            );

            var dashedLineLength = Height - scoreBoardHeight - borderWidth - 2;
            var sidewalkLine1Position = new Vector2(borderWidth + sidewalkWidth, scoreBoardHeight + borderWidth);
            var sidewalkLine2Position = new Vector2(Width - borderWidth - sidewalkWidth - 1, scoreBoardHeight + borderWidth);
            DashedLine.Draw(spriteBatch, sidewalkLine1Position, dashedLineLength, Color.White);
            DashedLine.Draw(spriteBatch, sidewalkLine2Position, dashedLineLength, Color.White);

            foreach (var entity in entities)
            {
                entity.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
