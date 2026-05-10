using TMPro;
using UnityEngine;

namespace _RouletteGame.Scripts.UI
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerBetAmountText;
        [SerializeField] private TextMeshProUGUI _playerMoneyText;

        private void Awake()
        {
            _playerBetAmountText.text = PlayerStats.PlayerBetAmount.ToString();
            _playerMoneyText.text = PlayerStats.PlayerMoney.ToString();
        }
    }
}