using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pedestrian
{
    public class Tombstone : IEntity
    {
        Texture2D texture;
        Vector2 origin;

        public Color Color { get; set; } = Color.White;
        public bool IsStatic { get; } = true;
        public Vector2 Position { get; private set; }
        public Collider Collider { get; }
        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                origin = new Vector2(value.Width / 2, value.Height / 2);
            }
        }

        public Tombstone(Vector2 position)
        {
            Position = position;
            Texture = PedestrianGame.Instance.Content.Load<Texture2D>("cross16bit01");
            Collider = new BoxCollider
            {
                Position = position,
                Width = Texture.Width - 7,
                Height = Texture.Height - 4
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
