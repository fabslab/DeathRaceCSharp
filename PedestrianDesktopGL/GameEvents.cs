using System.Collections.Generic;

namespace Pedestrian
{
    public enum GameEvents
    {
        GameStart,
        GameOver,
        EnemyKilled,
        PlayerScored,
        LoadMenu,
        Exit,
        Resume,
    }

    /// <summary>
	/// Comparer that should be passed to a Dictionary constructor to ensure 
    /// no boxing/unboxing when using an enum as a key on Mono
	/// </summary>
	public struct CoreEventsComparer : IEqualityComparer<GameEvents>
    {
        public bool Equals(GameEvents x, GameEvents y)
        {
            return x == y;
        }

        public int GetHashCode(GameEvents obj)
        {
            return ((int)obj).GetHashCode();
        }
    }
}
