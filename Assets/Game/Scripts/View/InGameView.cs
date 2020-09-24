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
            TileMapSystem.HexagonBlowed += OnHexagonBlowed;
        }

        private void UnregisterEvents()
        {
            TileMapSystem.HexagonBlowed -= OnHexagonBlowed;
        }
        private void InitializeElements()
        {
            _scoreElement.Initiliaze();
        }

        private void DisposeElements()
        {
            _scoreElement.Dispose();
        }

        private void OnHexagonBlowed(Vector2 screenPosition)
        {
            var floatingText = Instantiate(_floatingTextPrefab, transform);
            floatingText.rectTransform.position = screenPosition;
            floatingText.FadeAway(GameController.Instance.ScoreAmount);
        }
    }

    public class InGameViewParameters : ViewParameters { }
}