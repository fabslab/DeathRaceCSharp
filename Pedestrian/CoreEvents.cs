using System.Collections.Generic;

namespace Pedestrian
{
    public enum CoreEvents
    {
        EnemyKilled,
        GameOver
    }

    /// <summary>
	/// Comparer that should be passed to a Dictionary constructor to ensure 
    /// no boxing/unboxing when using an enum as a key on Mono
	/// </summary>
	public struct CoreEventsComparer : IEqualityComparer<CoreEvents>
    {
        public bool Equals(CoreEvents x, CoreEvents y)
        {
            return x == y;
        }

        public int GetHashCode(CoreEvents obj)
        {
            return ((int)obj).GetHashCode();
        }
    }
}
