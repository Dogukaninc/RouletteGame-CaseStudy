using System.Collections.Generic;
using _RouletteGame.Scripts;
using _RouletteGame.Scripts.Events;
using _RouletteGame.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RouletteGame.Scripts
{
    public class BetManager : MonoBehaviour
    {
        [SerializeField] private BetTile[] _allBets;

        [SerializeField] private Button _clearBetsButton;

        [SerializeField] private Button _chip1Button;
        [SerializeField] private Button _chip5Button;
        [SerializeField] private Button _chip10Button;
        [SerializeField] private Button _chip25Button;
        [SerializeField] private Button _chip100Button;
        [SerializeField] private Button _chip500Button;

        [SerializeField] private float _offsetMultiplier;

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

            _clearBetsButton.onClick.AddListener(ClearAllBet);
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

            _clearBetsButton.onClick.RemoveListener(ClearAllBet);
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

        private void PlaceChipOnBet(BetTile betTile)
        {
            if (!CanAddModeBet())
            {
                Debug.LogWarning("Can not add more bet !!!");
                return;
            }

            var chipObj = PoolSystem.Instance.SpawnGameObject(_selectedChipTag);
            betTile.ChipsOnBet.Add(chipObj);
            betTile.SetHighlight();

            var posOffset = new Vector3(0, betTile.GetChipsOnBetCount() * _offsetMultiplier, 0);
            chipObj.transform.position = betTile.transform.position + posOffset;
            AddBet(GetChipValueFromTag(_selectedChipTag));
        }

        private int GetChipValueFromTag(string tag)
        {
            switch (tag)
            {
                case "chip1":
                    return 1;
                case "chip5":
                    return 5;
                case "chip10":
                    return 10;
                case "chip25":
                    return 25;
                case "chip100":
                    return 100;
                case "chip500":
                    return 500;
                default:
                    Debug.LogError("Invalid chip tag: " + tag);
                    return 0;
            }
        }
        
        private List<BetTile> GetAllBets()
        {
            List<BetTile> selectedBets = new List<BetTile>();
            for (int i = 0; i < _allBets.Length; i++)
            {
                var bet = _allBets[i];
                if (bet.IsBetMatching())
                {
                    selectedBets.Add(bet);
                }
            }

            return selectedBets;
        }

        private void ClearAllBet()
        {
            foreach (var bet in _allBets)
            {
                bet.ChipsOnBet.RemoveAll(chip =>
                {
                    chip.gameObject.SetActive(false);
                    return true;
                });

                bet.ChipsOnBet.Clear();
                bet.SetHighlight();
            }

            PlayerStats.PlayerBetAmount = 0;
            EventManager.Publish(new OnBetChanged(PlayerStats.PlayerBetAmount));
        }

        private void AddBet(int betAmount)
        {
            PlayerStats.PlayerBetAmount += betAmount;
            EventManager.Publish(new OnBetChanged(PlayerStats.PlayerBetAmount));
        }

        private void CalculateRatioAccordingToBet() //Oyuncunun ne kadar kazanıp ne kadar kaybedeceğini hesaplayacağız
        {
        }

        private bool CanAddModeBet()
        {
            return PlayerStats.PlayerMoney - PlayerStats.PlayerBetAmount > 0;
        }
    }
}