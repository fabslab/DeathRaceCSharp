using System.Collections.Generic;
using System.Linq;

namespace Pedestrian.Engine.Collision
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
            var collider1 = entity1.Collider;
            var collider2 = entity2.Collider;
            return 
                entity1 != entity2 && 
                collider1 != null && collider2 != null &&
                collider1 != collider2 &&
                (collider1.CollisionFilter.HasFlag(collider2.Category)) &&
                (collider2.CollisionFilter.HasFlag(collider1.Category)) &&
                collider1.Collides(collider2);
        }

        public static void Update(IEnumerable<IEntity> entities)
        {
            var allEntities = entities.ToArray();
            var movingEntities = entities.Where(e => !e.IsStatic).ToArray();

            // Get all collisions first before notifying colliders to avoid any
            // changes inside collision handlers affecting subsequent collision checks
            for (int i = 0, l = movingEntities.Length; i < l; ++i)
            {
                var entity = movingEntities[i];
                entity.Collider.CurrentCollidingEntities = GetCollisions(entity, allEntities);
            }

            for (int i = 0, l = movingEntities.Length; i < l; ++i)
            {
                var entity = movingEntities[i];
                var collider = entity.Collider;
                var entered = Enumerable.Except(collider.CurrentCollidingEntities, collider.PreviousCollidingEntities);
                var exited = Enumerable.Except(collider.PreviousCollidingEntities, collider.CurrentCollidingEntities);
                if (entered.Any())
                {
                    collider.OnCollisionEntered?.Invoke(entered);
                }
                if (collider.CurrentCollidingEntities.Any())
                {
                    collider.OnCollision?.Invoke(collider.CurrentCollidingEntities);
                }
                if (exited.Any())
                {
                    collider.OnCollisionExited?.Invoke(exited);
                }
                collider.PreviousCollidingEntities = collider.CurrentCollidingEntities;
            }
        }
    }
}
