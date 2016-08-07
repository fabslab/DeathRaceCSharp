using Microsoft.Xna.Framework;

namespace Pedestrian
{
    /// <summary>
    /// Simple collider that collides using its bounding box.
    /// </summary>
    public class BoxCollider : Collider
    {
        public BoxCollider() {}

        public BoxCollider(Rectangle area)
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
