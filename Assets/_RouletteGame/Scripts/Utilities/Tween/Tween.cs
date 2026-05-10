using System;
using UnityEngine;

namespace _RouletteGame.Utilities
{
    public class Tween
    {
        internal readonly object Target;
        internal readonly float Duration;
        internal readonly Ease Ease;
        internal readonly Action<float> Setter;

        internal float Elapsed;
        internal Action OnCompleteCallback;
        internal bool IsAlive;
        internal int Loops = 1;
        internal int CompletedLoops;

        internal Tween(object target, float duration, Ease ease, Action<float> setter)
        {
            Target = target;
            Duration = duration;
            Ease = ease;
            Setter = setter;
            IsAlive = true;
        }

        public bool IsActive => IsAlive;

        public Tween OnComplete(Action callback)
        {
            OnCompleteCallback = callback;
            return this;
        }

        public Tween SetLoops(int loops)
        {
            Loops = loops;
            return this;
        }

        public void Kill()
        {
            IsAlive = false;
        }

        internal void Tick(float deltaTime)
        {
            Elapsed += deltaTime;
            float time = Duration <= 0f ? 1f : Mathf.Clamp01(Elapsed / Duration);
            Setter(Easing.Evaluate(Ease, time));

            if (time < 1f) return;

            if (Loops < 0)
            {
                Elapsed -= Duration;
                return;
            }

            CompletedLoops++;
            if (CompletedLoops < Loops)
            {
                Elapsed -= Duration;
                return;
            }

            IsAlive = false;
            OnCompleteCallback?.Invoke();
        }
    }
}