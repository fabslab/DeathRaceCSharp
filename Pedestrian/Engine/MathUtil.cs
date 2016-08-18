using Microsoft.Xna.Framework;
using System;

namespace Pedestrian.Engine
{
    public static class MathUtil
    {
        public static float AngleBetweenVectors(Vector2 from, Vector2 to)
        {
            return (float)(Math.Atan2(to.Y, to.X) - Math.Atan2(from.Y, from.X));
        }

        public static float Snap(float value, float increment)
        {
            return (float)Math.Round(value / increment) * increment;
        }

        public static bool EqualWithTolerance(float first, float second, float tolerance = 0.001f)
        {
            return Math.Abs(first - second) <= tolerance;
        }
    }
}
