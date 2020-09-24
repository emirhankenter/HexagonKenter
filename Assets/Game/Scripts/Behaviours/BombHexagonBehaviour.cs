using Game.Scripts.Behaviours;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Game.Scripts.Behaviours
{
    public class BombHexagonBehaviour : HexagonBehaviour
    {
        public static event Action Exploded;

        [SerializeField] private TextMeshProUGUI _countdownText;
        [SerializeField] private int _countdown = 5;

        private void Awake()
        {
            PlayerBehaviour.PlayerMoveOccured += OnPlayerMoveOccured;
            _countdownText.text = _countdown.ToString();
        }

        private void OnDestroy()
        {
            PlayerBehaviour.PlayerMoveOccured -= OnPlayerMoveOccured;
        }

        private void OnPlayerMoveOccured()
        {
            _countdown--;
            _countdownText.text = _countdown.ToString();

            if (_countdown <= 0)
            {
                Exploded?.Invoke();
            }
        }
    }
}