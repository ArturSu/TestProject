using System;
using TestProject.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace TestProject.Views
{
    public class BattleUI : MonoBehaviour, IBattleUI
    {
        public event Action LeavePressed;
        public event Action ContinuePressed;

        [SerializeField] private Text _resultMessage;
        [SerializeField] private Text _log;
        [SerializeField] private Button _leaveBattleButton;
        [SerializeField] private Button _continueButton;
        
        private bool _showLog;

        public void Activate(bool showLog)
        {
            gameObject.SetActive(true);
            _resultMessage.gameObject.SetActive(false);
            _log.text = String.Empty;
            _showLog = showLog;
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void AddLog(string message)
        {
            if (_showLog)
            {
                _log.text = message + "\n" + _log.text;
            }
        }

        public void ShowResultMessage(string message)
        {
            _resultMessage.text = message;
            _resultMessage.gameObject.SetActive(true);
        }

        private void Start()
        {
            _leaveBattleButton.onClick.AddListener(LeaveBattleButton_Clicked);
            _continueButton.onClick.AddListener(ContinueButton_Clicked);
        }

        private void ContinueButton_Clicked()
        {
            OnContinuePressed();
        }

        private void LeaveBattleButton_Clicked()
        {
            OnLeavePressed();
        }

        private void OnLeavePressed()
        {
            LeavePressed?.Invoke();
        }

        private void OnContinuePressed()
        {
            ContinuePressed?.Invoke();
        }
    }
}