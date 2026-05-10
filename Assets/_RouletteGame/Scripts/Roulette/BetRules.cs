using System.Collections.Generic;
using _RouletteGame.Utilities;
using UnityEngine;

namespace RouletteGame.Scripts
{
    public static class BetRules
    {
        private static readonly Dictionary<Enums.BetType, int> _betMultipliers = new Dictionary<Enums.BetType, int>
        {
            { Enums.BetType.Straight, 35 },
            { Enums.BetType.Split, 17 },
            { Enums.BetType.Street, 11 },
            { Enums.BetType.Corner, 8 },
            { Enums.BetType.SixLine, 5 },
            { Enums.BetType.Dozens, 2 },
            { Enums.BetType.Columns, 2 },
            { Enums.BetType.Red, 1 },
            { Enums.BetType.Black, 1 },
            { Enums.BetType.Even, 1 },
            { Enums.BetType.Odd, 1 },
            { Enums.BetType.High, 1 },
            { Enums.BetType.Low, 1 }
        };
        
        public static int GetPayout(Enums.BetType betType, int betAmount)
        {
            if (_betMultipliers.TryGetValue(betType, out int multiplier))
            {
                return betAmount + (betAmount * multiplier);
            }

            Debug.LogWarning("Unknown bet type: " + betType);
            return 0;
        }

        public static int GetNetProfit(Enums.BetType betType, int betAmount)
        {
            return _betMultipliers.ContainsKey(betType) ? betAmount * _betMultipliers[betType] : 0;
        }
    }
}