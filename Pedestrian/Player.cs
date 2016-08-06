using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
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
        public Color Color { get; set; } = Color.White;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public PlayerInput Input { get; set; } = new KeyboardInput();
        // Assume sprite was drawn pointing up for default direction
        public Vector2 InitialDirection { get; set; } = DirectionMap.DIRECTION_VECTORS[Direction.Up];
        // Rotation from InitialDirection vector in radians
        public float Rotation { get; set; } = 0;
        // Maximum turn value in one frame in radians
        public float MaxTurnAngle { get; set; } = MathHelper.PiOver4 / 6;
        public float RotationSnapValue { get; set; } = MathHelper.PiOver4 / 2;
        // Max number of pixels to move in one movement
        public float MaxSpeed { get; set; } = 3f;
        public float MaxReverseSpeed { get; set; } = 1f;
        public float CrashedSpeed { get; set; } = 0;
        // Num ms player will be stationary after crashing if not cancelled by reversing
        public int MaxCrashTime { get; set; } = 500;
        public bool IsCrashed { get; set; } = false;
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

        public Player(Vector2 position)
        {
            Position = position;
            Texture = PedestrianGame.Instance.Content.Load<Texture2D>("car16bit01");
            Collider = new BoxCollider
            {
                Position = Position,
                Width = Texture.Width - 5,
                Height = Texture.Height - 3,
                OnCollisionEnter = OnCollisionEnter
            };
            crashTimer = Timers.GetTimer(MaxCrashTime);
            crashTimer.Paused = true;
            crashTimer.OnTimerEnd = (() => IsCrashed = false);
        }

        public void OnCollisionEnter(IEnumerable<IEntity> entities)
        {
            Score += entities.Count(e => e is Enemy);

            if (entities.Any())
            {
                Crash();
            }
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
            var speed = MaxSpeed;
            if (IsCrashed)
            {
                speed = CrashedSpeed;
            }
            else if (throttle < 0) {
                speed = MaxReverseSpeed;
            }
            speed *= throttle;

            var turnRadians = Input.GetTurnAngleNormalized() * MaxTurnAngle;

            // Invert turn angle when in reverse so still 
            // move in corresponding direction in screen space
            if (speed < 0) { turnRadians = -turnRadians; }
            // Decrease turning speed by half when not accelerating
            else if (speed == 0) { turnRadians /= 2; }

            Rotation += turnRadians;
            snappedRotation = MathUtil.Snap(Rotation, RotationSnapValue);
            // Reset rotation back to snapped value if player stops turning
            if (turnRadians == 0) { Rotation = snappedRotation; }

            var rotationMatrix = Matrix.CreateRotationZ(snappedRotation);
            var movementDirection = Vector2.Transform(InitialDirection, rotationMatrix);
            var movement = speed * movementDirection;

            Position += movement;
            Collider.Position = Position;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: Texture,
                origin: origin,
                position: Position,
                rotation: snappedRotation,
                color: Color
            );
        }

        public void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Collider.Draw(spriteBatch);
        }
    }
}
