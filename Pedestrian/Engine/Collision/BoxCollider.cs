using Microsoft.Xna.Framework;
using System;

namespace Pedestrian.Engine.Collision
{
    /// <summary>
    /// Simple collider that collides using its bounding box.
    /// </summary>
    public class BoxCollider : Collider
    {
        public BoxCollider(Enum category, Enum collisionFilter) 
            : base(category, collisionFilter) {}

        public BoxCollider(Rectangle area, Enum category, Enum collisionFilter)
            : base(category, collisionFilter)
        {
            Position = new Vector2(area.Center.X, area.Center.Y);
            Width = area.Width;
            Height = area.Height;
        }

        public override bool Collides(BoxCollider other)
        {
            return Bounds.Intersects(other.Bounds);
        }

        public override bool Collides(ContainerCollider other)
        {
            return !other.Bounds.Contains(Bounds);
        }
    }
}
