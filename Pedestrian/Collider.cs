using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Pedestrian
{
    public abstract class Collider
    {
        // By default the collider will do nothing on collisions
        protected static Action<IEnumerable<IEntity>> defaultCollisionHandler = (entities => { });
        protected Rectangle bounds;
        protected Vector2 position;
        protected bool invalidBounds = true;

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

        public abstract bool Collides(BoxCollider other);
        public abstract bool Collides(ContainerCollider other);
        public bool Collides(Collider other)
        {
            return Collides((dynamic)other);
        }

        public void Clear()
        {
            Array.Clear(CurrentCollidingEntities, 0, CurrentCollidingEntities.Length);
            Array.Clear(PreviousCollidingEntities, 0, PreviousCollidingEntities.Length);
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
