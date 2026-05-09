using _RouletteGame.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace RouletteGame.Scripts
{
    public class RouletteViewMono : MonoBehaviour
    {
        private readonly uint[] _rouletteNumbers = new uint[]
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36
        };

        [SerializeField]
        private Transform rouletteWheelRectTransform;

        [SerializeField]
        private Button spinButton;

        [SerializeField]
        private float _rotationDuration;

        private Tween _idleRotationTween;

        private void OnEnable()
        {
            // spinButton.onClick.AddListener();
        }

        private void OnDisable()
        {
            // spinButton.onClick.AddListener();
        }

        private void Start()
        {
            RotateRouletteWheelIdle();
        }

        private void RotateRouletteWheelIdle()
        {
            if (_idleRotationTween != null && _idleRotationTween.IsActive)
            {
                return;
            }

            _idleRotationTween = rouletteWheelRectTransform.DoRotateBy(new Vector3(0, 0, 360f), _rotationDuration, Ease.Linear).SetLoops(-1);
        }

        private void SetSlotTransform(GameObject slot, int i, float height)
        {
            
        }
    }
}