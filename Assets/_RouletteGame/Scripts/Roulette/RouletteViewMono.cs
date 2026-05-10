using System;
using _RouletteGame.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace RouletteGame.Scripts
{
    public class RouletteViewMono : MonoBehaviour
    {
        private readonly uint[] _rouletteNumbers = new uint[]
        {
            0, 32, 15, 19, 4, 21, 2, 25, 17, 34,
            6, 27, 13, 36, 11, 30, 8, 23, 10, 5,
            24, 16, 33, 1, 20, 14, 31, 9, 22, 18,
            29, 7, 28, 12, 35, 3, 26
        };

        [SerializeField] private Transform rouletteWheelRectTransform;
        [SerializeField] private Transform _slotsParent;
        [SerializeField] private Transform _ball;
        [SerializeField] private Transform _ballMesh;
        [SerializeField] private float _slotOrbitRadius = 0.0275f;
        [SerializeField] private float _slotDistanceThresholdMultiplier = 1f;
        [SerializeField, Tooltip("0 = otomatik (360/slot sayısı). >0 ise iki slot arasındaki açıyı zorlar.")]
        private float _slotAngularSpacingDeg = 0f;
        [SerializeField] private float _rotationDuration;
        [SerializeField] private float _ballSpinDuration = 4f;
        [SerializeField, Min(1)] private int _ballSpinTurns = 5;
        [SerializeField] private int _debugSelectedWheelNumber;
        [SerializeField] private Button _spinButton;

        [Serializable]
        private struct WheelSlot
        {
            public int Number;
            public float LocalAngleDeg;
        }

        [SerializeField] private WheelSlot[] _slots;
        private Tween _idleRotationTween;

        private void Awake()
        {
            _spinButton.onClick.AddListener(RollRouletteBall);
        }

        private void OnDestroy()
        {
            _spinButton.onClick.RemoveListener(RollRouletteBall);
        }

        private void Start()
        {
            RotateRouletteWheelIdle();
            InitializeWheelSlots();
        }

        private void InitializeWheelSlots()
        {
            int count = _rouletteNumbers.Length;
            _slots = new WheelSlot[count];

            float anglePerSlot = _slotAngularSpacingDeg > 0f
                ? _slotAngularSpacingDeg
                : 360f / count;

            for (int i = 0; i < count; i++)
            {
                float angleDeg = i * anglePerSlot;
                float rad = angleDeg * Mathf.Deg2Rad;

                GameObject slot = PoolSystem.Instance.SpawnGameObject(Tags.POOL_TAG_ROULETTE_NUMBER);
                slot.transform.SetParent(_slotsParent, false);
                slot.transform.localPosition = new Vector3(
                    Mathf.Cos(rad) * _slotOrbitRadius * _slotDistanceThresholdMultiplier,
                    Mathf.Sin(rad) * _slotOrbitRadius * _slotDistanceThresholdMultiplier,
                    0f);
                Vector3 outward = (slot.transform.position - _slotsParent.position).normalized;
                slot.transform.rotation = Quaternion.LookRotation(Vector3.down, outward);
                slot.name = $"Slot_{_rouletteNumbers[i]}";

                TextMeshPro slotText = slot.GetComponentInChildren<TextMeshPro>();
                if (slotText != null)
                {
                    slotText.text = _rouletteNumbers[i].ToString();
                }

                _slots[i] = new WheelSlot
                {
                    Number = (int)_rouletteNumbers[i],
                    LocalAngleDeg = angleDeg
                };
            }
        }

        private void RotateRouletteWheelIdle()
        {
            if (_idleRotationTween != null && _idleRotationTween.IsActive)
            {
                return;
            }

            _idleRotationTween = rouletteWheelRectTransform.DoRotateBy(new Vector3(0, 360f, 0), _rotationDuration, Ease.Linear).SetLoops(-1);
        }

        public void RollRouletteBall()
        {
            int targetNumber = GetSelectedNumberOfRouletteWheel();
            int slotIndex = FindSlotIndexByNumber(targetNumber);
            if (slotIndex < 0 || _ball == null) return;

            Transform mesh = _ballMesh != null ? _ballMesh : (_ball.childCount > 0 ? _ball.GetChild(0) : null);
            if (mesh == null) return;

            Transform wheel = rouletteWheelRectTransform;
            Transform slotTransform = _slotsParent.GetChild(slotIndex);

            Vector3 slotDelta = slotTransform.position - _slotsParent.position;
            float slotCurrentWorldAngle = Mathf.Atan2(slotDelta.z, slotDelta.x) * Mathf.Rad2Deg;

            float wheelSpeedDegPerSec = _rotationDuration > 0f ? 360f / _rotationDuration : 0f;
            float slotPredictedWorldAngle = slotCurrentWorldAngle - wheelSpeedDegPerSec * _ballSpinDuration;

            Vector3 meshDelta = mesh.position - _ball.position;
            float meshCurrentWorldAngle = Mathf.Atan2(meshDelta.z, meshDelta.x) * Mathf.Rad2Deg;

            float startBallY = _ball.eulerAngles.y;
            float targetBallY = startBallY + meshCurrentWorldAngle - slotPredictedWorldAngle;

            float deltaToTarget = Mathf.DeltaAngle(startBallY, targetBallY);
            float endBallY = startBallY + (_ballSpinTurns * 360f) + deltaToTarget;

            TweenExtensions.DoFloat(startBallY, endBallY, _ballSpinDuration, y =>
            {
                Vector3 e = _ball.eulerAngles;
                _ball.eulerAngles = new Vector3(e.x, y, e.z);
            }, Ease.OutQuart).OnComplete(() =>
            {
                Debug.Log($"Ball landed on number {targetNumber}");
                _ball.SetParent(wheel, true);
            });
        }

        private int FindSlotIndexByNumber(int number)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Number == number) return i;
            }

            return -1;
        }

        private int GetRandomNumberOfRouletteWheel()
        {
            int randomIndex = Random.Range(0, _rouletteNumbers.Length);
            return (int)_rouletteNumbers[randomIndex];
        }

        private int GetSelectedNumberOfRouletteWheel()
        {
            return _debugSelectedWheelNumber;
        }
    }
}