using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Pedestrian
{
    public static class Collision
    {
        /// <summary>
        /// Returns list of entities that are colliding with the given entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="collisionEntities"></param>
        /// <returns></returns>
        public static IEnumerable<IEntity> GetCollisions(IEntity entity, IEnumerable<IEntity> collisionEntities)
        {
            return collisionEntities.Where(c => IsColliding(entity, c));
        }

        public static bool IsColliding(IEntity entity1, IEntity entity2)
        {
            return 
                entity1 != entity2 && 
                entity1.Collider != null &&
                entity2.Collider != null &&
                entity1.Collider != entity2.Collider &&
                entity1.Collider.Bounds.Intersects(entity2.Collider.Bounds);
        }

        public static void Update(IEnumerable<IEntity> entities)
        {
            // Get all collisions first before notifying colliders to avoid any
            // changes inside collision handlers affecting subsequent collision checks
            foreach (var entity in entities)
            {
                entity.Collider.CurrentCollidingEntities = GetCollisions(entity, entities).ToArray();
            }

            var collidingEntities = entities.ToArray();
            foreach (var entity in collidingEntities)
            {
                var collider = entity.Collider;
                var entered = Enumerable.Except(collider.CurrentCollidingEntities, collider.PreviousCollidingEntities);
                var exited = Enumerable.Except(collider.PreviousCollidingEntities, collider.CurrentCollidingEntities);
                if (entered.Any())
                {
                    collider.OnCollisionEnter(entered);
                }
                if (exited.Any())
                {
                    collider.OnCollisionExit(exited);
                }
                collider.PreviousCollidingEntities = collider.CurrentCollidingEntities;
            }
        }
    }
}
