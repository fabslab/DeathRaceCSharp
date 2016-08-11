using Microsoft.Xna.Framework;

namespace Pedestrian.Engine.Collision
{
    public class ContainerCollider : Collider
    {
        public ContainerCollider(Rectangle rectangle)
        {
            bounds = rectangle;
            invalidBounds = false;
        }

        public override bool Collides(BoxCollider other)
        {
            return !Bounds.Contains(other.Bounds);
        }

        public override bool Collides(ContainerCollider other)
        {
            // Doesn't make sense to have two container colliders collide
            return false;
        }
    }
}
