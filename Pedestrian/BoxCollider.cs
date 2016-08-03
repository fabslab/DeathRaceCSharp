namespace Pedestrian
{
    /// <summary>
    /// Simple collider that collides using its bounding box.
    /// </summary>
    public class BoxCollider : Collider
    {
        public override bool Collides(BoxCollider other)
        {
            return Bounds.Intersects(other.Bounds);
        }

        public override bool Collides(ContainerCollider other)
        {
            var isCollinding = !other.Bounds.Contains(Bounds);
            if (isCollinding)
            {
                isCollinding = true;
            }
            return isCollinding;
        }
    }
}
