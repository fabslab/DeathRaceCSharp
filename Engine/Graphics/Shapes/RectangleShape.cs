using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pedestrian.Engine.Graphics.Shapes
{
    public static class RectangleShape
    {
        public static void Draw(SpriteBatch spriteBatch, Rectangle area, Color color)
        {
            var colorPixel = PixelTextures.Instance.WhitePixel;
            spriteBatch.Draw(colorPixel, area, color);
        }
    }
}
