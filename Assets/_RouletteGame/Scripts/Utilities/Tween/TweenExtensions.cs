using System;
using UnityEngine;

namespace _RouletteGame.Utilities
{
    public static class TweenExtensions
    {
        public static Tween DoMove(this Transform target, Vector3 endValue, float duration, Ease ease = Ease.OutQuart)
        {
            Vector3 start = target.position;
            return Tweener.Instance.Register(new Tween(target, duration, ease,
                t => target.position = Vector3.LerpUnclamped(start, endValue, t)));
        }

        public static Tween DoLocalMove(this Transform target, Vector3 endValue, float duration, Ease ease = Ease.OutQuart)
        {
            Vector3 start = target.localPosition;
            return Tweener.Instance.Register(new Tween(target, duration, ease,
                t => target.localPosition = Vector3.LerpUnclamped(start, endValue, t)));
        }

        public static Tween DoScale(this Transform target, Vector3 endValue, float duration, Ease ease = Ease.OutQuart)
        {
            Vector3 start = target.localScale;
            return Tweener.Instance.Register(new Tween(target, duration, ease,
                t => target.localScale = Vector3.LerpUnclamped(start, endValue, t)));
        }

        public static Tween DoRotate(this Transform target, Vector3 endValue, float duration, Ease ease = Ease.OutQuart)
        {
            Vector3 start = target.eulerAngles;
            return Tweener.Instance.Register(new Tween(target, duration, ease,
                t => target.eulerAngles = Vector3.LerpUnclamped(start, endValue, t)));
        }

        public static Tween DoLocalRotate(this Transform target, Vector3 endValue, float duration, Ease ease = Ease.OutQuart)
        {
            Vector3 start = target.localEulerAngles;
            return Tweener.Instance.Register(new Tween(target, duration, ease,
                t => target.localEulerAngles = Vector3.LerpUnclamped(start, endValue, t)));
        }

        public static Tween DoRotateBy(this Transform target, Vector3 deltaEuler, float duration, Ease ease = Ease.OutQuart)
        {
            Vector3 start = target.eulerAngles;
            Vector3 end = start + deltaEuler;
            return Tweener.Instance.Register(new Tween(target, duration, ease,
                t => target.eulerAngles = Vector3.LerpUnclamped(start, end, t)));
        }

        public static Tween DoFloat(float from, float to, float duration, Action<float> setter, Ease ease = Ease.OutQuart)
        {
            return Tweener.Instance.Register(new Tween(null, duration, ease,
                t => setter(Mathf.LerpUnclamped(from, to, t))));
        }
    }
}