using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine;
using Pedestrian.Engine.Collision;
using Pedestrian.Engine.Graphics.Shapes;
using Pedestrian.Engine.Input;
using System.Collections.Generic;
using System.Linq;

namespace Pedestrian
{
    public class Player : IEntity
    {
        Texture2D texture;
        Vector2 origin;
        Vector2 initialPosition;
        float snappedRotation;
        float currentMaxSpeed;
        Timer crashTimer;
        SoundEffect collisionSound;
        SoundEffectInstance engineSound;

        public PlayerIndex PlayerIndex { get; private set; }
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
        public float DefaultMaxSpeed { get; set; } = 4f;
        public float MaxReverseSpeed { get; set; } = 2f;
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

        public Player(Vector2 initialPosition, PlayerIndex playerIndex)
        {
            currentMaxSpeed = DefaultMaxSpeed;
            PlayerIndex = playerIndex;
            this.initialPosition = initialPosition;
            Position = initialPosition;
            Input = new PlayerInput(playerIndex);
            Texture = PedestrianGame.Instance.Content.Load<Texture2D>("car16bit02");
            Collider = new BoxCollider(ColliderCategory.Default, ~ColliderCategory.GameBounds)
            {
                Position = Position,
                Width = Texture.Width - 4,
                Height = Texture.Height,
                OnCollisionEntered = OnCollisionEntered,
                OnCollisionExited = OnCollisionExited,
                OnCollision = OnCollision
            };
            crashTimer = Timers.GetTimer(MaxCrashTime);
            crashTimer.Paused = true;
            crashTimer.OnTimerEnd = (() => IsCrashed = false);

            collisionSound = PedestrianGame.Instance.Content.Load<SoundEffect>("Audio/collision");
            engineSound = PedestrianGame.Instance.Content.Load<SoundEffect>("Audio/engine-running").CreateInstance();
            engineSound.IsLooped = true;
        }

        public void OnCollisionEntered(IEnumerable<IEntity> entities)
        {
            if (entities.Any())
            {
                if (RoadBounds.Instance.Bounds.Contains(Position))
                {
                    if (entities.Any(e => !(e is Enemy)))
                    {
                        Crash();
                    }
                }
            }
        }

        public void IncrementScore()
        {
            Score++;
            PedestrianGame.Instance.Events.Emit(GameEvents.PlayerScored, this);
        }

        public void OnCollision(IEnumerable<IEntity> entities)
        {
            currentMaxSpeed = 1;
        }

        public void OnCollisionExited(IEnumerable<IEntity> entities)
        {
            currentMaxSpeed = DefaultMaxSpeed;
        }

        private void Crash()
        {
            collisionSound.Play();
            IsCrashed = true;
            crashTimer.Reset();
            crashTimer.Paused = false;
        }

        public void Update(GameTime time)
        {
            var throttle = Input.GetThrottleValue();
            var speed = currentMaxSpeed;
            if (throttle < 0)
            {
                // Reversing cancels crash
                IsCrashed = false;
                speed = MaxReverseSpeed;
            }
            else if (IsCrashed)
            {
                speed = 0;
            }
            speed *= throttle;

            UpdateEngineSound(speed);

            var turnRadians = Input.GetTurnAngleNormalized() * MaxTurnAngle;

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

        private void UpdateEngineSound(float speed)
        {
            if (speed != 0)
            {
                engineSound.Volume = 1;

                if (engineSound.State == SoundState.Stopped)
                {
                    engineSound.Play();
                }
            }
            else
            {
                if (engineSound.Volume < 0.1f)
                {
                    engineSound.Stop();
                }
                else
                {
                    engineSound.Volume -= 0.1f;
                }
            }
        }

        public void Reset()
        {
            Position = initialPosition;
            Collider.Position = initialPosition;
            Rotation = 0;
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
            RectangleShape.Draw(spriteBatch, new Rectangle(Position.ToPoint(), new Point(1, 1)), Color.Red);
        }

        public void Unload()
        {
            engineSound.Stop();
            Timers.RemoveTimer(crashTimer);
        }
    }
}
