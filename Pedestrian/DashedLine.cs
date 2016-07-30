using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pedestrian
{
    public static class DashedLine
    {
        private static Texture2D lineTexture;

        public static void Draw(SpriteBatch spriteBatch, Vector2 start, int length, Color color)
        {
            // assume once the line texture is created, it can be reused (game-specific)
            // ie. subsequent lines are the same length
            if (lineTexture == null)
            {
                lineTexture = new Texture2D(spriteBatch.GraphicsDevice, 2, length);

                // width of dashes in pixels
                var width = 2;
                // number of pixels to skip between dashes
                var space = width * 2;

                var pixelData = new Color[width * length];
                var l = pixelData.Length - width * space;
                var increment = width * space + space;
                for (var i = width * space; i < l; i += increment)
                {
                    // make square dashes
                    for (var j = i; j < i + width * width; ++j)
                    {
                        pixelData[j] = Color.White;
                    }
                }
                lineTexture.SetData<Color>(pixelData);
            }

            spriteBatch.Draw(lineTexture, start, color);
        }
    }
}
