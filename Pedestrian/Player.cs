using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine;
using Pedestrian.Engine.Collision;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Pedestrian
{
    public class Player : IEntity
    {
        Texture2D texture;
        Vector2 origin;
        float snappedRotation;
        Timer crashTimer;
        
        public int Score { get; private set; } = 0;
        public bool IsStatic { get; } = false;
        public Color Color { get; set; } = Color.White;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public IPlayerInput Input { get; set; }
        // Assume sprite was drawn pointing up for default direction
        public Vector2 InitialDirection { get; set; } = DirectionMap.DIRECTION_VECTORS[Direction.Up];
        // Rotation from InitialDirection vector in radians
        public float Rotation { get; set; } = 0;
        // Maximum turn value in one frame in radians
        public float MaxTurnAngle { get; set; } = MathHelper.PiOver4 / 6;
        public float RotationSnapValue { get; set; } = MathHelper.PiOver4 / 2;
        // Max number of pixels to move in one movement
        public float DefaultMaxSpeed { get; set; } = 3f;
        public float CurrentMaxSpeed { get; set; } = 3f;
        public float MaxReverseSpeed { get; set; } = 1f;
        // Num ms player will be stationary after crashing if not cancelled by reversing
        public int MaxCrashTime { get; set; } = 1500;
        public bool IsCrashed { get; set; } = false;
        // Player can not kill enemies when out of bounds (on sidewalk)
        public Collider Collider { get; }


        private Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                origin = new Vector2(value.Width / 2, value.Height / 2);
            }
        }

        public Player(Vector2 position, PlayerIndex playerIndex, IPlayerInput inputHandler = null)
        {
            Input = inputHandler ?? new KeyboardInput(KeyboardInputMap.GetInputMap(playerIndex));
            Position = position;
            Texture = PedestrianGame.Instance.Content.Load<Texture2D>("car16bit01");
            Collider = new BoxCollider(ColliderCategory.Default, ~ColliderCategory.GameBounds)
            {
                Position = Position,
                Width = Texture.Width - 5,
                Height = Texture.Height - 3,
                OnCollisionEntered = OnCollisionEntered,
                OnCollisionExited = OnCollisionExited,
                OnCollision = OnCollision
            };

            crashTimer = Timers.GetTimer(MaxCrashTime);
            crashTimer.Paused = true;
            crashTimer.OnTimerEnd = (() => IsCrashed = false);
        }

        public void OnCollisionEntered(IEnumerable<IEntity> entities)
        {
            if (entities.Any())
            {
                if (RoadBounds.Instance.Bounds.Contains(Position))
                {
                    foreach (Enemy enemy in entities.Where(e => e is Enemy))
                    {
                        enemy.Kill();
                        Score++;
                    }
                    Crash();
                }
            }
        }

        public void OnCollision(IEnumerable<IEntity> entities)
        {
            CurrentMaxSpeed = DefaultMaxSpeed / 3;
        }

        public void OnCollisionExited(IEnumerable<IEntity> entities)
        {
            CurrentMaxSpeed = DefaultMaxSpeed;
        }

        private void Crash()
        {
            IsCrashed = true;
            crashTimer.Reset();
            crashTimer.Paused = false;
        }

        public void Update(GameTime time)
        {
            var throttle = Input.GetThrottleValue();
            var speed = CurrentMaxSpeed;
            if (throttle < 0)
            {
                speed = MaxReverseSpeed;
                // Reversing cancels crash time
                IsCrashed = false;
            }
            else if (IsCrashed)
            {
                speed = 0;
            }
            speed *= throttle;

            var turnRadians = Input.GetTurnAngleNormalized() * MaxTurnAngle;

            // Invert turn angle when in reverse so still 
            // move in corresponding direction in screen space
            if (speed < 0) {
                turnRadians = -turnRadians;
            }

            Rotation += turnRadians;
            snappedRotation = MathUtil.Snap(Rotation, RotationSnapValue);
            // Reset rotation back to snapped value if player stops turning
            if (turnRadians == 0) { Rotation = snappedRotation; }

            var rotationMatrix = Matrix.CreateRotationZ(snappedRotation);
            var movementDirection = Vector2.Transform(InitialDirection, rotationMatrix);
            var movement = speed * movementDirection;

            Position = PositionUtil.WrapPosition(Position + movement, PlayArea.Instance.Bounds);
            Collider.Position = Position;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: Texture,
                origin: origin,
                position: Position,
                rotation: snappedRotation,
                color: Color,
                effects: SpriteEffects.None,
                scale: 1,
                layerDepth: 0,
                sourceRectangle: null
            );
        }

        public void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Collider.Draw(spriteBatch);
        }
    }
}
