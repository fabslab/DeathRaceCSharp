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
        public static IEntity[] GetCollisions(IEntity entity, IEnumerable<IEntity> collisionEntities)
        {
            return collisionEntities.Where(c => IsColliding(entity, c)).ToArray();
        }

        public static bool IsColliding(IEntity entity1, IEntity entity2)
        {
            return 
                entity1 != entity2 && 
                entity1.Collider != null &&
                entity2.Collider != null &&
                entity1.Collider != entity2.Collider &&
                entity1.Collider.Collides(entity2.Collider);
        }

        public static void Update(IEnumerable<IEntity> entities)
        {
            var entityArray = entities.ToArray();

            // Get all collisions first before notifying colliders to avoid any
            // changes inside collision handlers affecting subsequent collision checks
            for (int i = 0, l = entityArray.Length; i < l; ++i)
            {
                var entity = entityArray[i];
                entity.Collider.CurrentCollidingEntities = GetCollisions(entity, entityArray);
            }

            for (int i = 0, l = entityArray.Length; i < l; ++i)
            {
                var entity = entityArray[i];
                var collider = entity.Collider;
                var entered = Enumerable.Except(collider.CurrentCollidingEntities, collider.PreviousCollidingEntities);
                var exited = Enumerable.Except(collider.PreviousCollidingEntities, collider.CurrentCollidingEntities);
                if (entered.Any())
                {
                    collider.OnCollisionEnter?.Invoke(entered);
                }
                if (exited.Any())
                {
                    collider.OnCollisionExit?.Invoke(exited);
                }
                collider.PreviousCollidingEntities = collider.CurrentCollidingEntities;
            }
        }
    }
}
