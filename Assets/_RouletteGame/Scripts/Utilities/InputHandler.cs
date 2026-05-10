using RouletteGame.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _RouletteGame.Utilities
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _clickableLayer;

        private Camera _mainCamera;
        private readonly float _maxRayDistance = 200f;
        private readonly float _chipYOffset = 0.25f; // TODO -> Chip koydupumuz bet'in üzerinde kaç chip var ona bakmamız gerekiyor. bet'in üzerindeki chip sayısı kadar offset koyucaz

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
                var betTile = GetClickedBetTile();
                // Debug.Log("X: " + clickPointOnWorld.x +
                //           "\n Y:" + clickPointOnWorld.y +
                //           "\n Z: " + clickPointOnWorld.z);
                if (betTile == null)
                {
                    Debug.LogWarning("betTile null");
                    return;
                }
                
                GameStaticEvents.OnPlayerClickBet?.Invoke(betTile);
            }
        }

        private BetTile GetClickedBetTile()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _maxRayDistance, _clickableLayer))
            {
                if (hit.transform.TryGetComponent<BetTile>(out BetTile betTile))
                {
                    return betTile;
                }
            }

            return null;
        }
    }
}