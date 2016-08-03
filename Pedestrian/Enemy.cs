using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pedestrian
{
    public class Enemy : IEntity
    {
        static Random randomTurn = new Random();

        AnimatedTexture frontSprite;
        AnimatedTexture sideSprite;
        AnimatedTexture currentSprite;
        Vector2 initialDirection;
        Vector2 initialPosition;
        Vector2 previousPosition;
        int updatesSinceTurn = 0;

        // Min and max update cycles to wait before turning pedestrian in random direction
        public int[] IntervalRangeForTurn { get; set; } = new int[] { 15, 50 };
        public Color Color { get; set; } = Color.White;
        public Vector2 Position { get; set; }
        public float Speed { get; set; } = 3f;
        public Vector2 MovementDirection { get; set; }
        public Collider Collider { get; private set; }

        public Enemy(Vector2 enemyPosition)
        {
            Position = enemyPosition;
            initialPosition = enemyPosition;
            previousPosition = enemyPosition;
            
            initialDirection = DirectionMap.DIRECTION_VECTORS[Direction.Down];
            MovementDirection = initialDirection;

            frontSprite = new AnimatedTexture();
            frontSprite.Load("gremlin16bit02", 2, 50);
            sideSprite = new AnimatedTexture();
            sideSprite.Load("gremlin16bit-side01", 2, 50);
            currentSprite = frontSprite;

            Collider = new BoxCollider
            {
                Position = enemyPosition,
                Width = frontSprite.FrameWidth - 3,
                Height = frontSprite.FrameHeight - 1,
                OnCollisionEnter = OnCollisionEnter
            };
        }

        public void OnCollisionEnter(IEnumerable<IEntity> entities)
        {
            if (entities.Any(e => e is Player))
            {
                Scene.Events.Emit(CoreEvents.EnemyKilled, this);
                Die();
            }
            else if (entities.Any())
            {
                Position = previousPosition;
                MakeRandomTurn();
                Collider.Clear();
            }
        }

        public void Die()
        {
            // Death just resets the entity
            Position = initialPosition;
            Collider.Position = initialPosition;
            MovementDirection = initialDirection;
        }

        public void MakeRandomTurn()
        {
            Vector3 resultantDirection = Vector3.Cross(new Vector3(MovementDirection, 0), Vector3.UnitZ);

            var randomDirection = randomTurn.Next(0, 2);
            if (randomDirection == 0)
            {
                resultantDirection = -resultantDirection;
            }

            MovementDirection = new Vector2(resultantDirection.X, resultantDirection.Y);

            if (MovementDirection == DirectionMap.DIRECTION_VECTORS[Direction.Left] ||
                MovementDirection == DirectionMap.DIRECTION_VECTORS[Direction.Right])
            {
                currentSprite = sideSprite;
            }
            else
            {
                currentSprite = frontSprite;
            }

            updatesSinceTurn = 0;
        }

        public void Update(GameTime time)
        {
            updatesSinceTurn++;
            var shouldTurn = false;
            if (updatesSinceTurn >= IntervalRangeForTurn[1])
            {
                shouldTurn = true;
            }
            else if (updatesSinceTurn >= IntervalRangeForTurn[0])
            {
                // 5% chance to turn each update during allowed interval
                shouldTurn = randomTurn.Next(0, 20) == 0;
            }
            if (shouldTurn)
            {
                MakeRandomTurn();
            }

            previousPosition = Position;
            Position += MovementDirection * Speed;
            Collider.Position = Position;
            
            currentSprite.Update(time);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Assume side sprite is drawn facing left direction 
            // so flip if current direction is right
            SpriteEffects flipEffect = SpriteEffects.None;
            if (MovementDirection == DirectionMap.DIRECTION_VECTORS[Direction.Right])
            {
                flipEffect = SpriteEffects.FlipHorizontally;
            }

            currentSprite.Draw(spriteBatch, Position, flipEffect);
        }

        public void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Collider.Draw(spriteBatch);
        }
    }
}
