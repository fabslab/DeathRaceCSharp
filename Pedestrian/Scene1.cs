using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Pedestrian
{
    public class Scene1
    {
        List<IEntity> entities = new List<IEntity>();
        SpriteBatch spriteBatch;

        public int Width { get { return PedestrianGame.VIRTUAL_WIDTH; } }
        public int Height { get { return PedestrianGame.VIRTUAL_HEIGHT; } }

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

            // Initialize the enemies - place them in specific positions
            var enemy1Position = new Vector2((int)(Width * 0.25), (int)(Height * 0.2 + 17));
            var enemy1 = new PedestrianEnemy(enemy1Position);

            var enemy2Position = new Vector2((int)(Width * 0.75), (int)(Height * 0.2 + 17));
            var enemy2 = new PedestrianEnemy(enemy2Position);

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
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            Border.Draw(
                spriteBatch,
                new Rectangle(
                    0,
                    16,
                    Width,
                    Height - 17
                ),
                Color.White
            );
            DashedLine.Draw(spriteBatch, new Vector2(16, 17), Height - 18, Color.White);
            DashedLine.Draw(spriteBatch, new Vector2(Width - 16, 17), Height - 18, Color.White);

            foreach (var entity in entities)
            {
                entity.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
