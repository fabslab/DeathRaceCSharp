using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine;
using Pedestrian.Engine.Collision;
using Pedestrian.Engine.Graphics;
using Pedestrian.Engine.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pedestrian
{
    public class Enemy : IEntity
    {
        static Random randomTurn = new Random();

        AnimatedTexture frontSprite;
        AnimatedTexture leftSprite;
        AnimatedTexture rightSprite;
        AnimatedTexture currentSprite;
        Vector2 initialDirection;
        Vector2 movementDirection;
        Vector2 initialPosition;
        Vector2 previousPosition;
        int timeSinceTurn = 0;

        // Min and max ms to wait before enemy turns
        public int[] IntervalRangeForTurn { get; set; } = new int[] { 350, 1800 };
        public Color Color { get; set; } = Color.White;
        public bool IsStatic { get; } = false;
        public float Speed { get; set; } = 3f;
        public Collider Collider { get; }
        public Vector2 Position { get; private set; }
        public Vector2 MovementDirection
        {
            get { return movementDirection; }
            private set
            {
                movementDirection = value;
                timeSinceTurn = 0;
                UpdateSpriteDirection();
            }
        }

        public Enemy(Vector2 enemyPosition)
        {
            Position = enemyPosition;
            initialPosition = enemyPosition;
            previousPosition = enemyPosition;
            
            initialDirection = DirectionMap.DIRECTION_VECTORS[Direction.Down];
            MovementDirection = initialDirection;

            frontSprite = new AnimatedTexture();
            frontSprite.Load("gremlin16bit02", 2, 50);
            leftSprite = new AnimatedTexture();
            leftSprite.Load("gremlin16bit-left01", 2, 50);
            rightSprite = new AnimatedTexture();
            rightSprite.Load("gremlin16bit-right01", 2, 50);
            currentSprite = frontSprite;

            // Collides only with default and not other collider types
            Collider = new BoxCollider(ColliderCategory.Default, ColliderCategory.Default | ColliderCategory.GameBounds)
            {
                Position = enemyPosition,
                Width = frontSprite.FrameWidth - 3,
                Height = frontSprite.FrameHeight - 1,
                OnCollisionEntered = OnCollisionEnter
            };
        }

        public void OnCollisionEnter(IEnumerable<IEntity> entities)
        {
            if (entities.Any(e => !(e is Player)))
            {
                // Create chance to turn in opposite direction when hitting wall
                if (entities.Any(e => e is PlayArea) && randomTurn.Next(0, 2) == 0)
                {
                    MovementDirection = -MovementDirection;
                }
                else
                {
                    MakeRandomTurn();
                }
                Position = previousPosition;
                Collider.Clear();
            }
        }

        public void Kill()
        {
            PedestrianGame.Instance.Events.Emit(GameEvents.EnemyKilled, this);
            Reset();
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
        }

        private void UpdateSpriteDirection()
        {
            if (MovementDirection == DirectionMap.DIRECTION_VECTORS[Direction.Left])
            {
                currentSprite = leftSprite;
            }
            else if (MovementDirection == DirectionMap.DIRECTION_VECTORS[Direction.Right])
            {
                currentSprite = rightSprite;
            }
            else
            {
                currentSprite = frontSprite;
            }
        }

        public void Update(GameTime time)
        {
            timeSinceTurn += time.ElapsedGameTime.Milliseconds;
            var shouldTurn = false;
            if (timeSinceTurn >= IntervalRangeForTurn[1])
            {
                shouldTurn = true;
            }
            else if (timeSinceTurn >= IntervalRangeForTurn[0])
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

        public void Reset()
        {
            Position = initialPosition;
            Collider.Position = initialPosition;
            MovementDirection = initialDirection;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            currentSprite.Draw(spriteBatch, Position);
        }

        public void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Collider.Draw(spriteBatch);
        }
    }
}
