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
                lineTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, length);

                var pixelData = new Color[length];
                for (var i = 2; i < length; i += 3)
                {
                    pixelData[i] = Color.White;
                }
                lineTexture.SetData<Color>(pixelData);
            }

            spriteBatch.Draw(lineTexture, start, color);
        }
    }
}
