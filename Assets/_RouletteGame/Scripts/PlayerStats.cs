using UnityEngine;

namespace _RouletteGame.Scripts
{
    public class PlayerStats : MonoBehaviour
    {
        public static int PlayerBetAmount
        {
            get { return _playerBetAmount; }
            set { _playerBetAmount = value; }
        }

        public static int PlayerMoney
        {
            get { return _playerMoney; }
            set { _playerMoney = value; }
        }

        private static int _playerMoney = 1000;
        private static int _playerBetAmount = 0;
    }
}