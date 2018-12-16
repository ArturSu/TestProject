using System;
using TestProject.Controllers.SetupArmies;
using UnityEngine;
using UnityEngine.UI;

namespace TestProject.Views
{
    public class ArmySetupView : MonoBehaviour, IArmySetupView
    {
        public event Action<int, int> SetupCompleted;

        [SerializeField] private Text _playerText;
        [SerializeField] private Text _opponentText;
        [SerializeField] private Slider _playerSlider;
        [SerializeField] private Slider _opponentSlider;
        [SerializeField] private Button _startBattleButton;
        
        private int _playerSoldiers;
        private int _opponentSoldiers;
        
        public void Show(int playerMinSoldiers, int playerMaxSoldiers, int opponentMinSoldiers, int opponentMaxSoldiers)
        {
            _playerSlider.minValue = playerMinSoldiers;
            _playerSlider.maxValue = playerMaxSoldiers;

            _opponentSlider.minValue = opponentMinSoldiers;
            _opponentSlider.maxValue = opponentMaxSoldiers;
            _playerSoldiers = (playerMaxSoldiers + playerMinSoldiers) / 2;
            _opponentSoldiers = (opponentMaxSoldiers + opponentMinSoldiers) / 2;
            _playerSlider.value = _playerSoldiers;
            _opponentSlider.value = _opponentSoldiers;
            UpdateLabels();
            
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            _playerSlider.onValueChanged.AddListener(PlayerSlider_ValueChanged);
            _opponentSlider.onValueChanged.AddListener(OpponentSlider_ValueChanged);
            _startBattleButton.onClick.AddListener(StartBattleButton_Clicked);
        }

        private void UpdateLabels()
        {
            _playerText.text = $"Player soldiers : {_playerSoldiers}";
            _opponentText.text = $"AI soldiers : {_opponentSoldiers}";
        }
        
        private void StartBattleButton_Clicked()
        {
            OnSetupCompleted(_playerSoldiers, _opponentSoldiers);
        }

        private void OpponentSlider_ValueChanged(float value)
        {
            _opponentSoldiers = (int) value;
            UpdateLabels();
        }
        
        private void PlayerSlider_ValueChanged(float value)
        {
            _playerSoldiers = (int)value;
            UpdateLabels();
        }

        private void OnSetupCompleted(int playerSoldiers, int opponentSoldiers)
        {
            SetupCompleted?.Invoke(playerSoldiers, opponentSoldiers);
        }
    }
}