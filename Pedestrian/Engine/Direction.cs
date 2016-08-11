using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Pedestrian.Engine
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public static class DirectionMap
    {
        public static Dictionary<Direction, Vector2> DIRECTION_VECTORS = new Dictionary<Direction, Vector2>
        {
            { Direction.Up, -Vector2.UnitY },
            { Direction.Right, Vector2.UnitX },
            { Direction.Down, Vector2.UnitY },
            { Direction.Left, -Vector2.UnitX }
        };
    }
}
