using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Pedestrian
{
    public class PedestrianEnemy : IEntity
    {
        static Random randomTurn = new Random();

        AnimatedTexture sprite;
        Vector2 movementDirection;
        Vector2 initialPosition;
        Vector2 initialDirection;
        int updatesSinceTurn = 0;

        // Min and max update cycles to wait before turning pedestrian in random direction
        public int[] IntervalRangeForTurn { get; set; } = new int[] { 20, 60 };
        public Color Color { get; set; } = Color.White;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Speed { get; set; } = 1f;
        public Vector2 MovementDirection {
            get
            {
                return movementDirection;
            }
            set
            {
                movementDirection = value;
            }
        }
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)Position.X - sprite.FrameWidth / 2,
                    (int)Position.Y - sprite.FrameHeight / 2,
                    sprite.FrameWidth,
                    sprite.FrameHeight
                );
            }
        }

        public PedestrianEnemy(Vector2 position)
        {
            Position = position;
            initialPosition = position;
            initialDirection = DirectionMap.DIRECTION_VECTOR[Direction.Down];
            MovementDirection = initialDirection;
            sprite = new AnimatedTexture();
            sprite.Load("gremlin01", 2, 100);
        }

        public void Kill()
        {
            // Pedestrian death just resets position
            Position = initialPosition;
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
            var willCollide = !Scene.Bounds.Contains(this.Bounds);

            while (willCollide)
            {
                Position = previousPosition;
                MakeRandomTurn();
                Position += MovementDirection * Speed;
                willCollide = !Scene.Bounds.Contains(this.Bounds);
            }
        }

        public void MakeRandomTurn()
        {
            var randomDirection = randomTurn.Next(0, 2);
            Vector3 resultantDirection = Vector3.Cross(new Vector3(MovementDirection, 0), Vector3.UnitZ); ;
            if (randomDirection == 0)
            {
                resultantDirection = -resultantDirection;
            }
            movementDirection.X = resultantDirection.X;
            movementDirection.Y = resultantDirection.Y;
            updatesSinceTurn = 0;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.DrawFrame(spriteBatch, Position);
        }
    }
}
