using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine.Graphics;
using Pedestrian.Engine.Graphics.Shapes;

namespace Pedestrian.Engine.UI
{
    public class Button
    {
        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Text { get; set; }
        public Color Color { get; set; }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var area = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            RectangleShape.Draw(spriteBatch, area, Color);
        }
    }
}
