using System;
using System.Collections.Generic;

namespace Pedestrian.Engine
{
    /// <summary>
    /// Simple event emitter where generic contraint should be either an int or an enum
    /// </summary>
    public class EventEmitter<T> where T : struct, IComparable, IFormattable
    {
        // Match event type to functions listening for that event type
        Dictionary<T, List<Action>> eventTable;


        public EventEmitter()
        {
            eventTable = new Dictionary<T, List<Action>>();
        }


        /// <summary>
        /// If using some types of enums as the generic constraint you may want to
        /// pass in a custom comparer to avoid boxing/unboxing.
        /// </summary>
        /// <param name="customComparer">Custom comparer.</param>
        public EventEmitter(IEqualityComparer<T> customComparer)
        {
            eventTable = new Dictionary<T, List<Action>>(customComparer);
        }


        public void AddObserver(T eventType, Action handler)
        {
            List<Action> list = null;
            if (!eventTable.TryGetValue(eventType, out list))
            {
                list = new List<Action>();
                eventTable.Add(eventType, list);
            }
            list.Add(handler);
        }

        public void RemoveObserver(T eventType, Action handler)
        {
            eventTable[eventType].Remove(handler);
        }

        public void Emit(T eventType)
        {
            List<Action> list = null;
            if (eventTable.TryGetValue(eventType, out list))
            {
                for (int i = 0, l = list.Count; i < l; ++i)
                {
                    list[i]();
                }
            }
        }
    }


    /// <summary>
    /// Simple event emitter where generic contraint should be either an int or an enum
    /// and allows data to be passed with the event being emitted.
    /// </summary>
    public class EventEmitter<T, U> where T : struct, IComparable, IFormattable
    {
        // Match event type to functions listening for that event type
        Dictionary<T, List<Action<U>>> eventTable;

        public EventEmitter()
        {
            eventTable = new Dictionary<T, List<Action<U>>>();
        }

        /// <summary>
        /// If using some types of enums as the generic constraint you may want to
        /// pass in a custom comparer to avoid boxing/unboxing.
        /// </summary>
        /// <param name="customComparer">Custom comparer.</param>
        public EventEmitter(IEqualityComparer<T> customComparer)
        {
            eventTable = new Dictionary<T, List<Action<U>>>(customComparer);
        }

        public void AddObserver(T eventType, Action<U> handler)
        {
            List<Action<U>> list = null;
            if (!eventTable.TryGetValue(eventType, out list))
            {
                list = new List<Action<U>>();
                eventTable.Add(eventType, list);
            }
            list.Add(handler);
        }


        public void RemoveObserver(T eventType, Action<U> handler)
        {
            if (eventTable[eventType].Contains(handler))
            {
                eventTable[eventType].Remove(handler);
            }
        }


        public void Emit(T eventType, U data)
        {
            List<Action<U>> list = null;
            if (eventTable.TryGetValue(eventType, out list))
            {
                for (int i = 0, l = list.Count; i < l; ++i)
                {
                    list[i](data);
                }
            }
        }

    }

}
