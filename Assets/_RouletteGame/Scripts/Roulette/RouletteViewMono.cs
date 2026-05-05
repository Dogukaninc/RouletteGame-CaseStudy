using _RouletteGame.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace RouletteGame.Scripts
{
    public class RouletteViewMono : MonoBehaviour
    {
        [SerializeField] private Transform rouletteWheelRectTransform;
        [SerializeField] private Button spinButton;
        [SerializeField] private float _rotationDuration;
        
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
            
            _idleRotationTween = rouletteWheelRectTransform.DoRotateBy(new Vector3(0, 0,360f), _rotationDuration, Ease.Linear).SetLoops(-1);
        }

        private void SetSlotTransform(GameObject slot, int i, float height)
        {
           
        }
    }
}