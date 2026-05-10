using UnityEngine;

namespace RouletteGame.Scripts
{
    public class BetTile : MonoBehaviour
    {
        [SerializeField] private int [] _numbers;

        private void SetWinnerNumbers()
        {
            
        }

        public int[] GetWinnerNumbers()
        {
            return _numbers;
        }
    }
}