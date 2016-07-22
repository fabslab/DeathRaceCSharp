using System;

namespace Pedestrian
{
    public struct UnorderedPair<T, U> : IEquatable<UnorderedPair<T, U>>
    {
        T value1;
        U value2;

        public UnorderedPair(T val1, U val2)
        {
            value1 = val1;
            value2 = val2;
        }

        public bool Equals(UnorderedPair<T, U> other)
        {
            return 
                (value1.Equals(other.value1) && value2.Equals(other.value2)) || 
                (value1.Equals(other.value2) && value2.Equals(other.value1));
        }

        public bool Equals(T val1, U val2)
        {
            return
                (value1.Equals(val1) && value2.Equals(val2)) ||
                (value1.Equals(val2) && value2.Equals(val1));
        }

        public override int GetHashCode()
        {
            return value1.GetHashCode() ^ value2.GetHashCode();
        }
    }
}
