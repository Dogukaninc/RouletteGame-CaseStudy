using System.Collections.Generic;
using UnityEngine;

namespace RouletteGame.Scripts
{
    public class BetTile : MonoBehaviour
    {
        public List<GameObject> ChipsOnBet
        {
            get { return chipsOnBet; }
            set { chipsOnBet = value; }
        }

        [SerializeField] private List<GameObject> chipsOnBet;
        [SerializeField] private int[] _numbers;
        [SerializeField] private GameObject[] _highlights;
        
        private void SetWinnerNumbers()
        {
        }

        public int[] GetWinnerNumbers()
        {
            return _numbers;
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
            if (_highlights == null || _highlights.Length == 0)
                return;
            
            for (int i = 0; i < _highlights.Length; i++)
            {
                _highlights[i].SetActive(chipsOnBet.Count > 0);
            }
        }
    }
}