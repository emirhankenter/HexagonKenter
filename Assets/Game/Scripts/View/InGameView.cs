using Game.Scripts.Controllers;
using Game.Scripts.Helpers;
using Game.Scripts.View.Elements;
using Mek.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.View
{
    public class InGameView : View
    {
        [SerializeField] private ScoreElement _scoreElement;
        [SerializeField] private FloatingText _floatingTextPrefab;

        public override void Open(ViewParameters parameters)
        {
            base.Open();

            _scoreElement.Initiliaze();

            RegisterEvents();
        }

        public override void Close()
        {
            base.Close();

            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            TileMapSystem.HexagonBlowed += OnHexagonBlowed;
        }

        private void UnregisterEvents()
        {
            TileMapSystem.HexagonBlowed -= OnHexagonBlowed;
        }

        private void OnHexagonBlowed(Vector2 screenPosition)
        {
            var floatingText = Instantiate(_floatingTextPrefab, transform);
            floatingText.rectTransform.position = screenPosition;
            floatingText.FadeAway(GameController.Instance.ScoreAmount);

            _scoreElement.SetScore(GameController.CurrentScore);
        }
    }

    public class InGameViewParameters : ViewParameters { }
}