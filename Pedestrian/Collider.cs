using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Pedestrian
{
    public class Collider
    {
        Rectangle bounds;
        Vector2 position;
        bool dirtyBounds = true;

        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Offset { get; set; } = Vector2.Zero;

        public Rectangle Bounds
        {
            get
            {
                if (dirtyBounds)
                {
                    bounds = new Rectangle(
                        (int)(Position.X + Offset.X - Width / 2),
                        (int)(Position.Y + Offset.Y - Height / 2),
                        Width,
                        Height
                    );
                    dirtyBounds = false;
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
                dirtyBounds = true;
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

        public IEnumerable<Collider> Collisions { get; } = Enumerable.Empty<Collider>();
    }
}
