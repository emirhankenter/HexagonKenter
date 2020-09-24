using Game.Scripts.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Scripts.View.Elements
{
    public class ScoreElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        public void Initiliaze()
        {
            SetScore(0);
            PlayerData.ScoreChanged += OnScoreChanged;
        }

        public void Dispose()
        {
            PlayerData.ScoreChanged -= OnScoreChanged;
        }

        private void OnScoreChanged(int score)
        {
            SetScore(score);
        }

        public void SetScore(int score)
        {
            _scoreText.text = $"Score: {score}";
        }
    }
}