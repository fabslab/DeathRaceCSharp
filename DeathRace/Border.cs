using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pedestrian
{
    public static class Border
    {
        private static Texture2D pixel;

        public static void Draw(SpriteBatch spriteBatch, Rectangle area, Color color, int borderWidth = 1)
        {
            if (pixel == null)
            {
                pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                pixel.SetData<Color>(new Color[1] { Color.White });
            }

            spriteBatch.Draw(pixel, new Rectangle(area.X, area.Y, area.Width, borderWidth), color);
            spriteBatch.Draw(pixel, new Rectangle(area.X, area.Y, borderWidth, area.Height), color);
            spriteBatch.Draw(pixel, new Rectangle(area.X + area.Width - borderWidth, area.Y, borderWidth, area.Height), color);
            spriteBatch.Draw(pixel, new Rectangle(area.X, area.Y + area.Height - borderWidth, area.Width, borderWidth), color);
        }
    }
}