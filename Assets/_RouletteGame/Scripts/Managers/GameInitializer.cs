using System;
using _RouletteGame.Scripts;
using UnityEngine;

namespace RouletteGame.Scripts
{
    [DefaultExecutionOrder(-1)]
    public class GameInitializer : MonoBehaviour
    {
        private void Awake()
        {
            try
            {
                if (PlayerStats.PlayerBetAmount > 0)
                {
                    PlayerStats.PlayerBetAmount = 0;
                }

                if (PlayerStats.PlayerMoney <= 0)
                {
                    PlayerStats.PlayerMoney = 1000;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error initializing player stats: " + e.Message);
                PlayerStats.PlayerMoney = 1000; // Default value in case of error
            }
        }
    }
}