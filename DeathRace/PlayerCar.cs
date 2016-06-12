using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Pedestrian
{
    public class PlayerCar : IEntity
    {
        Texture2D texture;
        Vector2 origin;

        public Color Color { get; set; } = Color.White;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public PlayerInput Input { get; set; } = new KeyboardInput();
        // Assume sprite was drawn pointing up for default direction
        public Vector2 InitialDirection { get; set; } = -Vector2.UnitY;
        // Rotation from InitialDirection vector in radians
        public float Rotation { get; set; } = 0;
        // Maximum turn value in one frame in radians
        public float MaxTurnAngle { get; set; } = MathHelper.PiOver4 / 8;
        public float RotationSnapValue { get; set; } = MathHelper.PiOver4;
        // Max number of pixels to move in one movement
        public float MaxSpeed { get; set; } = 1f;
        public float MaxReverseSpeed { get; set; } = 0.5f;
        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                origin = new Vector2(value.Width / 2, value.Height / 2);
            }
        }

        public PlayerCar(Vector2 position)
        {
            Position = position;
            Texture = PedestrianGame.Instance.Content.Load<Texture2D>("car01");
        }

        public void Update(GameTime time)
        {
            var throttle = Input.GetThrottleValue();
            var speed = MaxSpeed;
            if (throttle < 0) {
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
            var snappedRotation = MathUtil.Snap(Rotation, RotationSnapValue);
            // Reset rotation back to snapped value if player stops turning
            if (turnRadians == 0) { Rotation = snappedRotation; }

            var rotationMatrix = Matrix.CreateRotationZ(snappedRotation);
            var movementDirection = Vector2.Transform(InitialDirection, rotationMatrix);
            var movement = speed * movementDirection;

            Position += movement;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var snappedRotation = MathUtil.Snap(Rotation, RotationSnapValue);
            spriteBatch.Draw(
                texture: Texture,
                origin: origin,
                position: Position,
                rotation: snappedRotation,
                color: Color
            );
        }
    }
}
