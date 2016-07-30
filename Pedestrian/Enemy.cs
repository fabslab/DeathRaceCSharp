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
        int updatesSinceTurn = 0;

        // Min and max update cycles to wait before turning pedestrian in random direction
        public int[] IntervalRangeForTurn { get; set; } = new int[] { 20, 60 };
        public Color Color { get; set; } = Color.White;
        public Vector2 Position { get; set; }
        public float Speed { get; set; } = 1.5f;
        public Vector2 MovementDirection { get; set; }
        public Collider Collider { get; private set; }

        public Enemy(Vector2 enemyPosition)
        {
            Position = enemyPosition;
            initialPosition = enemyPosition;
            
            initialDirection = DirectionMap.DIRECTION_VECTORS[Direction.Down];
            MovementDirection = initialDirection;

            frontSprite = new AnimatedTexture();
            frontSprite.Load("gremlin16bit02", 2, 50);
            sideSprite = new AnimatedTexture();
            sideSprite.Load("gremlin16bit-side01", 2, 50);
            currentSprite = frontSprite;

            Collider = new Collider
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
            var randomDirection = randomTurn.Next(0, 2);
            Vector3 resultantDirection = Vector3.Cross(new Vector3(MovementDirection, 0), Vector3.UnitZ);
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
            var mustTurn = false;
            var previousPosition = Position;

            if (updatesSinceTurn >= IntervalRangeForTurn[1])
            {
                mustTurn = true;
            }
            else if (updatesSinceTurn >= IntervalRangeForTurn[0])
            {
                // 5% chance to turn each update during allowed interval
                mustTurn = randomTurn.Next(0, 20) == 0;
            }

            if (mustTurn)
            {
                MakeRandomTurn();
            }

            Position += MovementDirection * Speed;
            Collider.Position = Position;
            var willCollide = !Scene.Bounds.Contains(Collider.Bounds);

            while (willCollide)
            {
                Position = previousPosition;
                MakeRandomTurn();
                Position += MovementDirection * Speed;
                Collider.Position = Position;
                willCollide = !Scene.Bounds.Contains(Collider.Bounds);
            }
            
            currentSprite.Update(time);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
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
