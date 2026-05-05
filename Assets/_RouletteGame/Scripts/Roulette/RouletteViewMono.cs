using UnityEngine;
using UnityEngine.UI;

namespace RouletteGame.Scripts
{
      public class RouletteViewMono : MonoBehaviour
    {

        [SerializeField] private RectTransform fortuneWheelRectTransform;
        [SerializeField] private Transform slotsParent;
        [SerializeField] private Button spinButton;
        
        private int _wheelSlotCount;
        private float _fortuneWheelRadius;
        private float _rotationDuration;
        private float _slotYPosOffset;
        private int _desiredSlotCount;
        private int _initialRotationCount;

        private void OnEnable()
        {
            spinButton.onClick.AddListener(SpinWheel);
        }

        private void OnDisable()
        {
            spinButton.onClick.AddListener(SpinWheel);
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            InitializeSlots();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
            }
        }

        private void InitializeSlots(float divider = 5)
        {
            float width = fortuneWheelRectTransform.rect.width;
            float height = fortuneWheelRectTransform.rect.height;
            _slotYPosOffset = height / divider;
            _fortuneWheelRadius = (Mathf.Min(width, height) * 0.5f) - _slotYPosOffset;

            for (int i = 0; i < _desiredSlotCount; i++)
            {
                // var slot = PoolSystem.Instance.SpawnGameObject("WheelSlot");
                // var wheelSlot = slot.GetComponent<WheelSlot>();
                // slot.transform.SetParent(slotsParent);
                // SelectRandomSlotConfig(wheelSlot);
                //
                // wheelSlot.SlotIndex = i;
                // wheelSlot.Initialize();
                //
                // SetSlotTransform(slot, i, height);
                //
                // if (wheelSlots.Contains(wheelSlot)) continue;
                // wheelSlots.Add(wheelSlot);
            }

            // _wheelSlotCount = wheelSlots.Count;
        }

        private void SetSlotTransform(GameObject slot, int i, float height)
        {
            var slotRect = slot.GetComponent<RectTransform>();

            float angle = i * (360 / _desiredSlotCount) + 90f;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(rad) * _fortuneWheelRadius, Mathf.Sin(rad) * _fortuneWheelRadius, 0f);

            slotRect.localPosition = pos;
            slotRect.rotation = Quaternion.Euler(0, 0, angle - 90f);
            var slotScaleFactor = height / slotRect.rect.height;
            slotRect.localScale = Vector3.one * (slotScaleFactor / 10);
        }

        private void SpinWheel()
        {
            // if (_sequence.IsActive() || _sequence.IsPlaying()) return;
            // _sequence?.Kill();
            // _sequence = DOTween.Sequence();
            //
            // float targetRotationAngle = CalculateTargetRotationAngle();
            // float initialRotation = (360f * _initialRotationCount);
            //
            // _sequence.Append(
            //     fortuneWheelRectTransform
            //         .DORotate(new Vector3(0, 0, initialRotation + targetRotationAngle), _rotationDuration, RotateMode.FastBeyond360)
            //         .SetEase(spinningEase)
            // );
            // _sequence.OnUpdate(() => wheelIndicatorController.IndicatorRoutine());
            // _sequence.AppendCallback(() => ShowPrizeCard(_selectedSlot.wheelSlotSo));
            // _sequence.AppendCallback(() => GeneralEvents.CallInitializeDisplayElement?.Invoke(_selectedSlot.wheelSlotSo.SlotIcon, 0, _selectedSlot.wheelSlotSo.UnitType));
            // _sequence.AppendCallback(() => GeneralEvents.OnDisplayElementUpdated?.Invoke(_selectedSlot.wheelSlotSo.UnitType, _selectedSlot.wheelSlotSo.SlotRewardInfoValue));
        }

        // private float CalculateTargetRotationAngle()
        // {
        //     _selectedSlot = SelectSlot();
        //     var targetSlotIndex = _selectedSlot.SlotIndex;
        //     var slotAngle = 360f / _wheelSlotCount;
        //     var targetRotationAngle = -slotAngle * targetSlotIndex;
        //     return targetRotationAngle;
        // }
        //
        //
        // private WheelSlot SelectSlot()
        // {
        //     var targetIndex = Random.Range(0, _wheelSlotCount);
        //     return wheelSlots.Find(s => s.SlotIndex == targetIndex);
        // }
        //
        // private void SelectRandomSlotConfig(WheelSlot wheelSlot)
        // {
        //     var slotConfig = wheelSlotDataHolderSo.AllWheelSlotData[Random.Range(0, wheelSlotDataHolderSo.AllWheelSlotData.Count)];
        //     wheelSlot.wheelSlotSo = slotConfig;
        // }
    }
}