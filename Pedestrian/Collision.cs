using Microsoft.Xna.Framework;
using System.Collections.Generic;
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
            if (entity.Collider == null)
            {
                return Enumerable.Empty<IEntity>();
            }

            return collisionEntities.Where(c =>
                    c != entity &&
                    c.Collider != null &&
                    c.Collider.Bounds.Intersects(entity.Collider.Bounds)
                );
        }

        public static bool IsColliding(IEntity entity1, IEntity entity2)
        {
            return 
                entity1 != entity2 && 
                entity1.Collider != null &&
                entity2.Collider != null &&
                entity1.Collider.Bounds.Intersects(entity2.Collider.Bounds);
        }

        public static void Update(
                IEnumerable<Player> players, 
                IEnumerable<Enemy> enemies, 
                IEnumerable<Tombstone> tombstones
            )
        {
            foreach (var player in players)
            {
                var playerCollisions = GetCollisions(player, players);
                var enemyCollisions = GetCollisions(player, enemies);
            }
        }
    }
}
