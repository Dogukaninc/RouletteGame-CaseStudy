using System;
using System.Collections.Generic;

namespace _RouletteGame.Utilities
{
    public static class EventManager
    {
        private static readonly Dictionary<Type, Action<object>> Events = new Dictionary<Type, Action<object>>();

        public static void Subscribe<T>(Action<T> action)
        {
            Type type = typeof(T);
            if (!Events.ContainsKey(type))
            {
                Events[type] = null;
            }
            Events[type] += (obj) => action((T)obj);
        }

        public static void Unsubscribe<T>(Action<T> action)
        {
            Type type = typeof(T);
            if (Events.ContainsKey(type))
            {
                Events[type] -= (obj) => action((T)obj);
            }
        }

        public static void Publish<T>(T eventData)
        {
            Type type = typeof(T);
            if (Events.ContainsKey(type))
            {
                Events[type]?.Invoke(eventData);
            }
        }
    }
}