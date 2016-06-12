using Microsoft.Xna.Framework;
using System;

namespace Pedestrian
{
    public static class MathUtil
    {
        static public float AngleBetweenVectors(Vector2 from, Vector2 to)
        {
            return (float)(Math.Atan2(to.Y, to.X) - Math.Atan2(from.Y, from.X));
        }

        static public float Snap(float value, float increment)
        {
            return (float)Math.Round(value / increment) * increment;
        }

        static public bool EqualWithTolerance(float first, float second, float tolerance = 0.001f)
        {
            return Math.Abs(first - second) <= tolerance;
        }
    }
}
