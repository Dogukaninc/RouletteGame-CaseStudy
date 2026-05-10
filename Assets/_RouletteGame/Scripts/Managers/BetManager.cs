using _RouletteGame.Scripts;
using _RouletteGame.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RouletteGame.Scripts
{
    public class BetManager : MonoBehaviour
    {
        [SerializeField] private Button _chip1Button;
        [SerializeField] private Button _chip5Button;
        [SerializeField] private Button _chip10Button;
        [SerializeField] private Button _chip25Button;
        [SerializeField] private Button _chip100Button;
        [SerializeField] private Button _chip500Button;

        private UnityAction _chip1Action, _chip5Action, _chip10Action, _chip25Action, _chip100Action, _chip500Action;
        private string _selectedChipTag;

        private void OnEnable()
        {
            GameStaticEvents.OnPlayerClickBet += PlaceChipOnBet;
            _chip1Action = () => SelectChipToSpawn(1);
            _chip5Action = () => SelectChipToSpawn(5);
            _chip10Action = () => SelectChipToSpawn(10);
            _chip25Action = () => SelectChipToSpawn(25);
            _chip100Action = () => SelectChipToSpawn(100);
            _chip500Action = () => SelectChipToSpawn(500);

            _chip1Button.onClick.AddListener(_chip1Action);
            _chip5Button.onClick.AddListener(_chip5Action);
            _chip10Button.onClick.AddListener(_chip10Action);
            _chip25Button.onClick.AddListener(_chip25Action);
            _chip100Button.onClick.AddListener(_chip100Action);
            _chip500Button.onClick.AddListener(_chip500Action);
        }

        private void OnDisable()
        {
            GameStaticEvents.OnPlayerClickBet -= PlaceChipOnBet;
            
            _chip1Button.onClick.RemoveListener(_chip1Action);
            _chip5Button.onClick.RemoveListener(_chip5Action);
            _chip10Button.onClick.RemoveListener(_chip10Action);
            _chip25Button.onClick.RemoveListener(_chip25Action);
            _chip100Button.onClick.RemoveListener(_chip100Action);
            _chip500Button.onClick.RemoveListener(_chip500Action);
        }

        private void SelectChipToSpawn(int chipValue)
        {
            switch (chipValue)
            {
                case 1:
                    _selectedChipTag = Tags.POOL_TAG_CHIP_1;
                    break;
                case 5:
                    _selectedChipTag = Tags.POOL_TAG_CHIP_5;
                    break;
                case 10:
                    _selectedChipTag = Tags.POOL_TAG_CHIP_10;
                    break;
                case 25:
                    _selectedChipTag = Tags.POOL_TAG_CHIP_25;
                    break;
                case 100:
                    _selectedChipTag = Tags.POOL_TAG_CHIP_100;
                    break;
                case 500:
                    _selectedChipTag = Tags.POOL_TAG_CHIP_500;
                    break;

                default:
                    Debug.LogError("Invalid chip value selected: " + chipValue);
                    break;
            }
        }

        private void PlaceChipOnBet(Vector3 position)
        {
            var chipObj = PoolSystem.Instance.SpawnGameObject(_selectedChipTag);
            chipObj.transform.position = position;
        }

        private void RemoveBet(int betAmount)
        {
            PlayerStats.PlayerBetAmount -= betAmount;
        }

        private void AddBet(int betAmount)
        {
            PlayerStats.PlayerBetAmount += betAmount;
        }
    }
}