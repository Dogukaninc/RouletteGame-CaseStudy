using System;
using _RouletteGame.Scripts;
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
        [SerializeField] private Transform _resultPoint;
        [SerializeField] private Transform _ballMesh;
        [SerializeField] private float _slotOrbitRadius = 0.0275f;
        [SerializeField] private float _slotDistanceThresholdMultiplier = 1f;
        [SerializeField] private float _slotAngularSpacingDeg = 0f;
        [SerializeField] private float _rotationDuration;
        [SerializeField] private float _ballSpinDuration = 4f;
        [SerializeField, Min(1)] private int _ballSpinTurns = 5;
        [SerializeField] private float _ballJumpHeight = 0.04f;
        [SerializeField] private float _ballJumpStepDuration = 0.2f;
        [SerializeField] private float _ballResetDuration = 0.6f;
        [SerializeField] private int _debugSelectedWheelNumber;
        [SerializeField] private Button _spinButton;
        [SerializeField] private TMP_InputField _debugNumberInput;
        

        
        [Serializable]
        private struct WheelSlot
        {
            public int Number;
            public float LocalAngleDeg;
        }

        [SerializeField] private WheelSlot[] _slots;
        private Tween _idleRotationTween;
        private BetManager _betManager;
        private Transform _ballInitialParent;
        private Vector3 _ballInitialLocalPosition;

        private void Awake()
        {
            _spinButton.onClick.AddListener(RollRouletteBall);

            _betManager = ServiceLocator.GetService<BetManager>();

            if (_debugNumberInput != null)
            {
                _debugNumberInput.onEndEdit.AddListener(SetDebugSelectedNumber);
            }

            if (_ball != null)
            {
                _ballInitialParent = _ball.parent;
                _ballInitialLocalPosition = _ball.localPosition;
            }
        }

        private void OnDestroy()
        {
            _spinButton.onClick.RemoveListener(RollRouletteBall);

            if (_debugNumberInput != null)
            {
                _debugNumberInput.onEndEdit.RemoveListener(SetDebugSelectedNumber);
            }
        }

        private void SetDebugSelectedNumber(string input)
        {
            if (!int.TryParse(input, out int number)) return;
            if (Array.IndexOf(_rouletteNumbers, (uint)number) < 0)
            {
                Debug.LogWarning($"Number {number} is not on the wheel");
                return;
            }
            _debugSelectedWheelNumber = number;
        }

        private void Start()
        {
            RotateRouletteWheelIdle();
            InitializeWheelSlots();
        }
        
        public void SetNumberDebugInput(string input)
        
        {
            if (int.TryParse(input, out int number))
            {
                _debugSelectedWheelNumber = number;
            }
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
                JumpBallToResultPoint(() => OnRouletteStopped(targetNumber));
            });
        }

        private void JumpBallToResultPoint(Action onComplete)
        {
            if (_resultPoint == null)
            {
                onComplete?.Invoke();
                return;
            }

            _ball.SetParent(null, true);

            Vector3 startPos = _ball.position;
            Vector3 endPos = _resultPoint.position;
            Vector3 apex = Vector3.Lerp(startPos, endPos, 0.5f) + Vector3.up * _ballJumpHeight;

            _ball.DoMove(apex, _ballJumpStepDuration, Ease.Linear)
                .OnComplete(() =>
                {
                    _ball.DoMove(_resultPoint.position, _ballJumpStepDuration, Ease.Linear)
                        .OnComplete(() =>
                        {
                            _ball.SetParent(_resultPoint, false);
                            onComplete?.Invoke();
                        });
                });
        }

        private void ResetBall()
        {
            if (_ball == null) return;

            _ball.SetParent(_ballInitialParent, true);
            _ball.DoLocalMove(_ballInitialLocalPosition, _ballResetDuration, Ease.InOutSine);
        }

        private void OnRouletteStopped(int winningNumber)
        {
            var allBets = _betManager.GetAllBets();
            int totalStaked = 0;
            int totalReturn = 0;

            for (int i = 0; i < allBets.Count; i++)
            {
                var bet = allBets[i];
                totalStaked += bet.totalBetAmount;

                if (Array.IndexOf(bet.GetWinnerNumbers(), winningNumber) >= 0)
                {
                    totalReturn += BetRules.GetPayout(bet.betType, bet.totalBetAmount);
                }
            }

            int netProfit = totalReturn - totalStaked;
            PlayerStats.PlayerMoney += netProfit;

            _betManager.ResetRound();
            Invoke(nameof(ResetBall), 2f);
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