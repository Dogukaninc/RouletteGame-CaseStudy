using System.Collections.Generic;
using UnityEngine;

namespace _RouletteGame.Utilities
{
    public class Tweener : MonoBehaviour
    {
        public static Tweener Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                
                var obj = new GameObject("[Tweener]");
                _instance = obj.AddComponent<Tweener>();
                DontDestroyOnLoad(obj);
                return _instance;
            }
        }

        private static Tweener _instance;
        private readonly List<Tween> _active = new List<Tween>(32);

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        internal Tween Register(Tween tween)
        {
            _active.Add(tween);
            return tween;
        }

        public void Kill(object target)
        {
            for (int i = 0; i < _active.Count; i++)
            {
                if (_active[i].Target == target)
                {
                    _active[i].Kill();
                }
            }
        }

        public void KillAll()
        {
            for (int i = 0; i < _active.Count; i++)
            {
                _active[i].Kill();
            }
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var tween = _active[i];
                if (!tween.IsAlive)
                {
                    _active.RemoveAt(i);
                    continue;
                }
                
                tween.Tick(deltaTime);
                
                if (!tween.IsAlive)
                {
                    _active.RemoveAt(i);
                }
            }
        }
    }
}
