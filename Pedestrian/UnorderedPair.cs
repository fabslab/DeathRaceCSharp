using System;

namespace Pedestrian
{
    public struct UnorderedPair<T, U> : IEquatable<UnorderedPair<T, U>>
    {
        public T Value1 { get; }
        public U Value2 { get; }

        public UnorderedPair(T val1, U val2)
        {
            Value1 = val1;
            Value2 = val2;
        }

        public bool Equals(UnorderedPair<T, U> other)
        {
            return 
                (Value1.Equals(other.Value1) && Value2.Equals(other.Value2)) || 
                (Value1.Equals(other.Value2) && Value2.Equals(other.Value1));
        }

        public bool Equals(T val1, U val2)
        {
            return
                (Value1.Equals(val1) && Value2.Equals(val2)) ||
                (Value1.Equals(val2) && Value2.Equals(val1));
        }

        public override int GetHashCode()
        {
            return Value1.GetHashCode() ^ Value2.GetHashCode();
        }
    }
}
