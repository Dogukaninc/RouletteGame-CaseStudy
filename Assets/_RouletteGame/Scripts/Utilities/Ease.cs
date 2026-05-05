using UnityEngine;

namespace _RouletteGame.Utilities
{
    public enum Ease
    {
        Linear,
        OutCubic,
        OutQuart,
        InOutSine,
        OutBack
    }

    public static class Easing
    {
        public static float Evaluate(Ease ease, float t)
        {
            switch (ease)
            {
                case Ease.Linear:
                    return t;
                case Ease.OutCubic:
                {
                    float u = 1f - t;
                    return 1f - u * u * u;
                }
                case Ease.OutQuart:
                {
                    float u = 1f - t;
                    return 1f - u * u * u * u;
                }
                case Ease.InOutSine:
                    return -(Mathf.Cos(Mathf.PI * t) - 1f) * 0.5f;
                case Ease.OutBack:
                {
                    const float c1 = 1.70158f;
                    const float c3 = c1 + 1f;
                    float u = t - 1f;
                    return 1f + c3 * u * u * u + c1 * u * u;
                }
                default:
                    return t;
            }
        }
    }
}