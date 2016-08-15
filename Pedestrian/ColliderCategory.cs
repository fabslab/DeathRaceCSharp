using System;

namespace Pedestrian
{
    /// <summary>
    /// Defined collider categories for collision filtering using bit masks.
    /// Supports up to 31 categories. (Using default signed 32-bit integer.)
    /// Currently defines only Sidewalk and Other.
    /// </summary>
    [Flags]
    public enum ColliderCategory
    {
        None = 0,
        Default = 1 << 0,
        RoadBounds = 1 << 1,
        GameBounds = 1 << 2,
        All = ~None
    }
}
