using System.Collections.Generic;
using _RouletteGame.Utilities;
using UnityEngine;

namespace RouletteGame.Scripts
{
    public class BetTile : MonoBehaviour
    {
        public int totalBetAmount { get; set; }
        public Enums.BetType betType;

        public List<GameObject> ChipsOnBet
        {
            get { return chipsOnBet; }
            set { chipsOnBet = value; }
        }

        [SerializeField] private List<GameObject> chipsOnBet;
        [SerializeField] private int[] _numbers;
        [SerializeField] private GameObject[] _highlights;

        public int[] GetWinnerNumbers()
        {
            return _numbers;
        }

        public void CalculateBetAmountOnTile(int amount)
        {
            totalBetAmount += amount;
        }

        public bool IsBetMatching()
        {
            return chipsOnBet.Count > 0;
        }

        public int GetChipsOnBetCount()
        {
            return chipsOnBet.Count;
        }

        public void SetHighlight()
        {
            if (_highlights == null || _highlights.Length <= 0)
                return;

            for (int i = 0; i < _highlights.Length; i++)
            {
                _highlights[i].SetActive(chipsOnBet.Count > 0);
            }
        }

        public void ResetBet()
        {
            for (int i = 0; i < chipsOnBet.Count; i++)
            {
                chipsOnBet[i].SetActive(false);
            }

            chipsOnBet.Clear();
            totalBetAmount = 0;
            SetHighlight();
        }

        private void SetWinnerNumbers()
        {
        }
    }
}