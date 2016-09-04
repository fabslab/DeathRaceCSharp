using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine;
using Pedestrian.Engine.Collision;

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
            Collider = new BoxCollider(ColliderCategory.Default, ColliderCategory.All)
            {
                Position = position,
                Width = Texture.Width - 7,
                Height = Texture.Height - 2
            };
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: Texture,
                origin: origin,
                position: Position,
                color: Color,
                effects: SpriteEffects.None,
                rotation: 0,
                scale: 1,
                sourceRectangle: null,
                layerDepth: 0
            );
        }

        public void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Collider.Draw(spriteBatch);
        }

        public void Update(GameTime time) { }
        public void Reset() { }
    }
}
