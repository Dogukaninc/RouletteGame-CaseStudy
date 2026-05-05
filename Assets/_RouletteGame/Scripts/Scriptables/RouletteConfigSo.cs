using UnityEngine;

namespace RouletteGame.Scripts
{
    [CreateAssetMenu(menuName = "Scriptable Objects/so_config_roulette", fileName = "so_config_roulette")]
    public class RouletteConfigSo : ScriptableObject
    {
        public float rotationDuration;
        public int initialRotationCount;
    }
}