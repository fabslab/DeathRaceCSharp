using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine;
using Pedestrian.Engine.Collision;

namespace Pedestrian
{
    /// <summary>
    /// Simple entity to represent an area that doesn't move but is involved in colllisions.
    /// </summary>
    public class AreaEntity : IEntity
    {
        public Collider Collider { get; }
        public Vector2 Position { get; }
        public bool IsStatic { get; } = true;

        public AreaEntity(Rectangle area, ColliderCategory colliderCategory)
        {
            Position = new Vector2(area.Center.X, area.Center.Y);
            Collider = new BoxCollider(area)
            {
                Category = colliderCategory
            };
        }

        public void Update(GameTime gameTime) { }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {}

        public void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Collider.Draw(spriteBatch);
        }
    }
}
