using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine.Graphics;

namespace Pedestrian.Engine.Collision
{
    public class ContainerCollider : Collider
    {
        public ContainerCollider(Rectangle rectangle, Enum category, Enum collisionFilter)
            : base(category, collisionFilter)
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

        public override void Draw(SpriteBatch spriteBatch, Color color)
        {
            // Since collision occurs with the outside of the container
            // draw debug box around the outside of the bounds
            Border.Draw(
                spriteBatch,
                Bounds,
                color,
                drawOutside: true
            );
        }
    }
}
