using UnityEngine;

namespace _RouletteGame.Utilities
{
    public class PoolableObject : MonoBehaviour
    {
        public string PoolTag { get; set; }

        private void OnDisable()
        {
            transform.localScale = Vector3.one;
            PoolSystem.Instance.ReturnToPool(PoolTag, gameObject);
        }
    }
}