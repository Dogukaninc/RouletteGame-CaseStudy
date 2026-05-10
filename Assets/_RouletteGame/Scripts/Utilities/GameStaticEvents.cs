using System;
using RouletteGame.Scripts;
using UnityEngine;

namespace _RouletteGame.Utilities
{
    public class GameStaticEvents : MonoBehaviour
    {
        public static Action<BetTile> OnPlayerClickBet;
    }
}