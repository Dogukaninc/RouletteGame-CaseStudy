using UnityEngine;
using UnityEngine.EventSystems;

namespace _RouletteGame.Utilities
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _clickableLayer;
        [SerializeField] private float _tableHeight;

        private Camera _mainCamera;
        private Plane _tablePlane;
        private readonly float _maxRayDistance = 200f;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _tablePlane = new Plane(Vector3.up, new Vector3(0f, _tableHeight, 0f));
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.GetMouseButtonDown(0))
            {
                var clickPointOnWorld = GetRayHitPoint();
                Debug.Log("X: " + clickPointOnWorld.x + 
                          "\n Y:" + clickPointOnWorld.y);
            }
        }

        private Vector2 GetRayHitPoint()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (_tablePlane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                var flatPosition = new Vector3(hitPoint.x, 0, hitPoint.z);
                return flatPosition;
            }

            return Vector2.zero;
        }
    }
}