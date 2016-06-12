using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pedestrian
{
    public class PedestrianEnemy : IEntity
    {
        AnimatedTexture sprite;

        public Color Color { get; set; } = Color.White;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 InitialDirection { get; set; } = Vector2.UnitY;
        public Vector2 CurrentDirection { get; set; } = Vector2.UnitY;
        public float Speed { get; set; } = 1f;

        public PedestrianEnemy(Vector2 position)
        {
            Position = position;
            sprite = new AnimatedTexture();
            sprite.Load("gremlin01", 2, 100);
        }

        public void Update(GameTime time)
        {
            sprite.UpdateFrame((float)time.ElapsedGameTime.TotalMilliseconds);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.DrawFrame(spriteBatch, Position);
        }
    }
}
