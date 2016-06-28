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
            return collisionEntities.Where(c => c != entity && c.Bounds.Intersects(entity.Bounds));
        }

        public static bool IsColliding(IEntity entity1, IEntity entity2)
        {
            return entity1 != entity2 && entity1.Bounds.Intersects(entity2.Bounds);
        }

        public static void Update(IEnumerable<PlayerCar> players, IEnumerable<PedestrianEnemy> enemies)
        {
            foreach (var player in players)
            {
                var playerCollisions = GetCollisions(player, players);
                var enemyCollisions = GetCollisions(player, enemies);

                if (!playerCollisions.Any() && !enemyCollisions.Any())
                {
                    player.IsCollided = false;
                }
                else
                {
                    foreach (PedestrianEnemy enemy in enemyCollisions)
                    {
                        enemy.Kill();
                        // TODO: Create tombstone
                        player.Score++;
                    }

                    if (!player.IsCollided)
                    {
                        player.Crash();
                    }
                    player.IsCollided = true;
                }
            }
        }
    }
}
