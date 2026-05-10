using UnityEngine;

namespace RouletteGame.Scripts
{
    [CreateAssetMenu(menuName = "Scriptable Objects/so_config_roulette", fileName = "so_config_roulette")]
    public class RouletteConfigSo : ScriptableObject
    {
        public float slotOrbitRadius = 0.0275f;
        public float slotDistanceThresholdMultiplier = 1f;
        public float slotAngularSpacingDeg = 0f;
        public float rotationDuration;
        public float ballSpinDuration = 4f;
        [Min(1)] public int ballSpinTurns = 5;
        public float ballJumpHeight = 0.04f;
        public float ballJumpStepDuration = 0.2f;
        public float ballResetDuration = 0.6f;
        public int debugSelectedWheelNumber;
    }
}