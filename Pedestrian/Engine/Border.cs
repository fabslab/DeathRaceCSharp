using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pedestrian.Engine
{
    public static class Border
    {
        private static Texture2D pixel;

        public static void Draw(SpriteBatch spriteBatch, Rectangle area, Color color, int borderWidth = 1, bool drawOutside = false)
        {
            if (pixel == null)
            {
                pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                pixel.SetData<Color>(new Color[1] { Color.White });
            }

            Rectangle leftLine, rightLine, topLine, bottomLine;
            if (drawOutside)
            {
                topLine = new Rectangle(area.X - borderWidth, area.Y - borderWidth, area.Width + borderWidth * 2, borderWidth);
                rightLine = new Rectangle(area.X + area.Width, area.Y, borderWidth, area.Height);
                leftLine = new Rectangle(area.X - borderWidth, area.Y, borderWidth, area.Height);
                bottomLine = new Rectangle(area.X - borderWidth, area.Y + area.Height, area.Width + borderWidth * 2, borderWidth);
            }
            else
            {
                topLine = new Rectangle(area.X, area.Y, area.Width, borderWidth);
                rightLine = new Rectangle(area.X + area.Width - borderWidth, area.Y, borderWidth, area.Height);
                leftLine = new Rectangle(area.X, area.Y, borderWidth, area.Height);
                bottomLine = new Rectangle(area.X, area.Y + area.Height - borderWidth, area.Width, borderWidth);
            }

            spriteBatch.Draw(pixel, leftLine, color);
            spriteBatch.Draw(pixel, topLine, color);
            spriteBatch.Draw(pixel, rightLine, color);
            spriteBatch.Draw(pixel, bottomLine, color);
        }
    }
}