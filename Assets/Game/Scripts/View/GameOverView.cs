using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View
{
    public class GameOverView : View
    {
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
        public Action OnComplete;

        public GameOverViewParameters(Action onComplete)
        {
            OnComplete = onComplete;
        }
    }
}