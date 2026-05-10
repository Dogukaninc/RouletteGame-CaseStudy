using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RouletteGame.Scripts
{
    [DefaultExecutionOrder(-200)]
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

#if UNITY_EDITOR
        public static void DebugPrintAllServices()
        {
            if (services.Count == 0)
            {
                Debug.Log("[ServiceLocator] No services are currently registered.");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[ServiceLocator] Active Services:");
            foreach (var kvp in services)
            {
                sb.AppendLine($" - {kvp.Key.Name}");
            }

            Debug.Log(sb.ToString());
        }
#endif
        
        public static void RegisterService<T>(T service) where T : class
        {
            var type = typeof(T);
            if (services.ContainsKey(type))
            {
                throw new InvalidOperationException($"Service of type {type.Name} is already registered.");
            }

            services[type] = service;
        }

        public static T GetService<T>() where T : class
        {
            var type = typeof(T);
            if (services.TryGetValue(type, out var service))
            {
                return service as T;
            }

            Debug.LogError($"{type} is not registered in the service locator !!!");
            return null;
        }
        
        public static bool TryGetService<T>(out T service) where T : class
        {
            if (services.TryGetValue(typeof(T), out var obj))
            {
                service = obj as T;
                return true;
            }

            service = null;
            return false;
        }

        public static bool IsRegistered<T>() where T : class
        {
            return services.ContainsKey(typeof(T));
        }

        public static void UnregisterService<T>() where T : class
        {
            var type = typeof(T);
            if (services.ContainsKey(type))
            {
                services.Remove(type);
            }
            else
            {
                Debug.LogWarning($"Tried to unregister service of type {type.Name}, but it was not registered.");
            }
        }
    }
}