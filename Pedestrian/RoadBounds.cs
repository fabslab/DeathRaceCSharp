using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine;
using Pedestrian.Engine.Collision;

namespace Pedestrian
{
    public class RoadBounds : IEntity
    {
        public static RoadBounds Instance { get; private set; }

        public Collider Collider { get; }
        public Vector2 Position { get; }
        public bool IsStatic { get; } = true;
        public Rectangle Bounds { get; private set; }

        public RoadBounds(Rectangle area)
        {
            Instance = this;
            Bounds = area;
            Collider = new ContainerCollider(area, ColliderCategory.RoadBounds, ColliderCategory.All);
        }

        public void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Collider.Draw(spriteBatch);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {}
        public void Update(GameTime gameTime) {}
        public void Reset() {}
    }
}
