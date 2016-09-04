using Microsoft.Xna.Framework;

namespace Pedestrian.Engine
{
    public static class PositionUtil
    {
        public static Vector2 WrapPosition(Vector2 position, Rectangle bounds)
        {
            var x = position.X;
            var y = position.Y;
            if (position.X < bounds.Left)
            {
                x = bounds.Right;
            }
            if (position.X > bounds.Right)
            {
                x = bounds.Left + 1;
            }
            if (position.Y < bounds.Top)
            {
                y = bounds.Bottom - 1;
            }
            if (position.Y > bounds.Bottom)
            {
                y= bounds.Top + 1;
            }
            return new Vector2(x, y);
        }
    }
}
