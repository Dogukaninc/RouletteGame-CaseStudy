using System.Reflection;
using UnityEngine;

namespace RouletteGame.Scripts
{
    [DefaultExecutionOrder(-100)]
    public class ServiceManager : MonoBehaviour
    {
        private void Awake()
        {
            AutoRegisterAllServices();
        }

        private void OnDestroy()
        {
            AutoUnregisterAllServices();
        }

        private void AutoRegisterAllServices()
        {
            var fields = GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (field.GetCustomAttributes(typeof(RegisterServiceAttribute), false).Length > 0)
                {
                    var value = field.GetValue(this);
                    if (value != null)
                    {
                        var type = field.FieldType;
                        typeof(ServiceLocator).GetMethod("RegisterService")
                            ?.MakeGenericMethod(type)
                            .Invoke(null, new object[] { value });
                    }
                }
            }
        }

        private void AutoUnregisterAllServices()
        {
            var fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.GetCustomAttribute<RegisterServiceAttribute>() != null)
                {
                    var type = field.FieldType;
                    typeof(ServiceLocator)
                        .GetMethod(nameof(ServiceLocator.UnregisterService))
                        ?.MakeGenericMethod(type)
                        .Invoke(null, null);
                }
            }
        }
    }
}