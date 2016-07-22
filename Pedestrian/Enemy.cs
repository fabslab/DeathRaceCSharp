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

        AnimatedTexture sprite;
        Vector2 initialDirection;
        Vector2 initialPosition;
        int updatesSinceTurn = 0;

        // Min and max update cycles to wait before turning pedestrian in random direction
        public int[] IntervalRangeForTurn { get; set; } = new int[] { 20, 60 };
        public Color Color { get; set; } = Color.White;
        public Vector2 Position { get; set; }
        public float Speed { get; set; } = 1f;
        public Vector2 MovementDirection { get; set; }
        public Collider Collider { get; private set; }

        public Enemy(Vector2 enemyPosition)
        {
            Position = enemyPosition;
            initialPosition = enemyPosition;

            initialDirection = DirectionMap.DIRECTION_VECTOR[Direction.Down];
            MovementDirection = initialDirection;

            sprite = new AnimatedTexture();
            sprite.Load("gremlin01", 2, 100);

            Collider = new Collider
            {
                Position = enemyPosition,
                Width = sprite.FrameWidth - 2,
                Height = sprite.FrameHeight
            };
        }

        public void OnCollisionEnter(IEnumerable<IEntity> entities)
        {
            if (entities.Any(e => e is Player))
            {
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

        public void Update(GameTime time)
        {
            sprite.UpdateFrame((float)time.ElapsedGameTime.TotalMilliseconds);

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
            updatesSinceTurn = 0;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.DrawFrame(spriteBatch, Position);
        }

        public void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Collider.Draw(spriteBatch);
        }
    }
}
