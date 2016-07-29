using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pedestrian
{
    public class Tombstone : IEntity
    {
        Texture2D texture;
        Vector2 origin;

        public Color Color { get; set; } = Color.White;
        public Vector2 Position { get; private set; }
        public Collider Collider { get; }
        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                origin = new Vector2(value.Width / 2 - 1, value.Height / 2);
            }
        }

        public Tombstone(Vector2 position)
        {
            Position = position;    
            Texture = PedestrianGame.Instance.Content.Load<Texture2D>("cross01");
            Collider = new Collider
            {
                Position = position,
                Width = Texture.Width - 3,
                Height = Texture.Height
            };
        }

        public void Update(GameTime time) {}

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                    texture: Texture,
                    origin: origin,
                    position: Position,
                    color: Color
                );
        }

        public void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Collider.Draw(spriteBatch);
        }
    }
}
