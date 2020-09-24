using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View
{
    public class GameOverView : View
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Button _restartButton;

        private GameOverViewParameters _params;

        public override void Open(ViewParameters parameters)
        {
            base.Open();

            _params = parameters as GameOverViewParameters;
            if (_params == null) return; 

            InitializeElements();

            RegisterEvents();
        }

        public override void Close()
        {
            base.Close();

            DisposeElements();

            UnregisterEvents();
        }

        private void RegisterEvents()
        {
        }

        private void UnregisterEvents()
        {
        }
        private void InitializeElements()
        {
            _restartButton.interactable = true;
            _scoreText.text = $"Score: {_params.Score.ToString()}";
        }

        private void DisposeElements()
        {
        }

        public void OnRestartButtonClicked()
        {
            _restartButton.interactable = false;
            _params.OnComplete?.Invoke();
        }
    }

    public class GameOverViewParameters : ViewParameters 
    {
        public int Score;
        public Action OnComplete;

        public GameOverViewParameters(int score, Action onComplete)
        {
            Score = score;
            OnComplete = onComplete;
        }
    }
}