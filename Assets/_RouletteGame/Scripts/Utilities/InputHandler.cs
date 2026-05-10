using UnityEngine;
using UnityEngine.EventSystems;

namespace _RouletteGame.Utilities
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _clickableLayer;

        private Camera _mainCamera;
        private readonly float _maxRayDistance = 200f;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.GetMouseButtonDown(0))
            {
                var clickPointOnWorld = GetRayHitPoint();
                Debug.Log("X: " + clickPointOnWorld.x +
                          "\n Y:" + clickPointOnWorld.y +
                          "\n Z: " + clickPointOnWorld.z);
            }
        }

        private Vector3 GetRayHitPoint()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _maxRayDistance, _clickableLayer))
            {
                return hit.point;
            }

            return Vector3.zero;
        }
    }
}