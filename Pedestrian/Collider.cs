using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Pedestrian
{
    public class Collider
    {
        // By default the collider will do nothing on collisions
        static Action<IEnumerable<IEntity>> defaultCollisionHandler = (entities => { });
        Rectangle bounds;
        Vector2 position;
        bool invalidBounds = true;

        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Offset { get; set; } = Vector2.Zero;
        public IEntity[] PreviousCollidingEntities { get; set; } = new IEntity[] { };
        public IEntity[] CurrentCollidingEntities { get; set; } = new IEntity[] { };
        public Action<IEnumerable<IEntity>> OnCollisionEnter { get; set; } = defaultCollisionHandler;
        public Action<IEnumerable<IEntity>> OnCollisionExit { get; set; } = defaultCollisionHandler;

        public Rectangle Bounds
        {
            get
            {
                if (invalidBounds)
                {
                    bounds = new Rectangle(
                        (int)(Position.X + Offset.X - Width / 2),
                        (int)(Position.Y + Offset.Y - Height / 2),
                        Width,
                        Height
                    );
                    invalidBounds = false;
                }
                return bounds;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                invalidBounds = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Color.Red);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            Border.Draw(
                spriteBatch,
                Bounds,
                color
            );
        }
    }
}
