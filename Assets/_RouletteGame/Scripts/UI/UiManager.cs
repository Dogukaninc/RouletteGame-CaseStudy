using _RouletteGame.Scripts.Events;
using _RouletteGame.Utilities;
using TMPro;
using UnityEngine;

namespace _RouletteGame.Scripts.UI
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerBetAmountText;
        [SerializeField] private TextMeshProUGUI _playerMoneyText;

        private void OnEnable()
        {
            EventManager.Subscribe<OnBetChanged>(UpdateBetText);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<OnBetChanged>(UpdateBetText);
        }

        private void Start()
        {
            EventManager.Publish(new OnBetChanged(PlayerStats.PlayerBetAmount));
            UpdateMoneyText();
        }

        private void UpdateBetText(OnBetChanged obj)
        {
            _playerBetAmountText.text = PlayerStats.PlayerBetAmount.ToString();
        }

        private void UpdateMoneyText()
        {
            _playerMoneyText.text = PlayerStats.PlayerMoney.ToString();
        }
    }
}