using Assets.Game.Scripts.Behaviours;
using Game.Scripts.Behaviours;
using Game.Scripts.Helpers;
using Game.Scripts.View;
using Mek.Controllers;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GameController : MonoBehaviour
    {
        public static event Action GameStarted;
        public static event Action GameOver;
        public Action BombSpawnScoreThresholdReached;

        [SerializeField, BoxGroup("GridSize")] private int _gridSizeX;
        [SerializeField, BoxGroup("GridSize")] private int _gridSizeY;

        public int ScoreAmount = 5;
        public static int BombSpawnThreshold = 15;

        [ReadOnly] public HexagonLevelBehaviour CurrentLevel;

        private CameraController _cameraController;
        public CameraController CameraController
        {
            get
            {
                if (_cameraController == null)
                {
                    _cameraController = GetComponent<CameraController>();
                }
                return _cameraController;
            }
        }

        private void Awake()
        {
            PrepareLevel();
        }

        private void PrepareLevel()
        {
            CurrentLevel = new GameObject("TileMap").AddComponent<HexagonLevelBehaviour>();

            CurrentLevel.Initiliaze(_gridSizeX, _gridSizeY);

            GameStarted?.Invoke();

            ViewController.Instance.InGameView.Open(new InGameViewParameters());

            TileMapSystem.HexagonBlowed += OnHexagonBlowed;
            BombHexagonBehaviour.Exploded += OnBombExploded;
        }

        private void DisposeLevel()
        {
            TileMapSystem.HexagonBlowed -= OnHexagonBlowed;
            BombHexagonBehaviour.Exploded -= OnBombExploded;

            Destroy(CurrentLevel.gameObject);

            CoroutineController.DoAfterGivenTime(1f, () =>
            {
                ViewController.Instance.GameOverView.Close();
                PrepareLevel();
            });
        }

        private void OnHexagonBlowed(Vector2 screenPosition)
        {
            PlayerData.CurrentScore += ScoreAmount;
        }

        private void OnBombExploded()
        {
            OnGameOver();
        }

        private void OnGameOver()
        {
            ViewController.Instance.InGameView.Close();
            ViewController.Instance.GameOverView.Open(new GameOverViewParameters(OnRestart));
            GameOver?.Invoke();
        }

        private void OnRestart()
        {
            DisposeLevel();
        }

        #region Singleton

        private static GameController _instance;

        public static GameController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameController>();

                    if (_instance == null)
                    {
                        Debug.LogError($"{typeof(GameController)} is needed in the scene but it does not exist!");
                    }
                }
                return _instance;
            }
        }

        #endregion
    }
}