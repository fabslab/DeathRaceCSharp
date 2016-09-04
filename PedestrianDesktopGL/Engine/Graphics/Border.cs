using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine.Graphics.Shapes;

namespace Pedestrian.Engine.Graphics
{
    public static class Border
    {
        public static void Draw(SpriteBatch spriteBatch, Rectangle area, Color color, int borderWidth = 1, bool drawOutside = false)
        {
            Draw(spriteBatch, area.Location, area.Width, area.Height, color, borderWidth, drawOutside);
        }

        public static void Draw(SpriteBatch spriteBatch, Point point, int width, int height, Color color, int borderWidth = 1, bool drawOutside = false)
        {
            Rectangle leftLine, rightLine, topLine, bottomLine;
            if (drawOutside)
            {
                topLine = new Rectangle(point.X - borderWidth, point.Y - borderWidth, width + borderWidth * 2, borderWidth);
                rightLine = new Rectangle(point.X + width, point.Y, borderWidth, height);
                leftLine = new Rectangle(point.X - borderWidth, point.Y, borderWidth, height);
                bottomLine = new Rectangle(point.X - borderWidth, point.Y + height, width + borderWidth * 2, borderWidth);
            }
            else
            {
                topLine = new Rectangle(point.X, point.Y, width, borderWidth);
                rightLine = new Rectangle(point.X + width - borderWidth, point.Y, borderWidth, height);
                leftLine = new Rectangle(point.X, point.Y, borderWidth, height);
                bottomLine = new Rectangle(point.X, point.Y + height - borderWidth, width, borderWidth);
            }

            RectangleShape.Draw(spriteBatch, leftLine, color);
            RectangleShape.Draw(spriteBatch, topLine, color);
            RectangleShape.Draw(spriteBatch, rightLine, color);
            RectangleShape.Draw(spriteBatch, bottomLine, color);
        }
    }
}