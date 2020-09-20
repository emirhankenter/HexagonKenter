﻿using Game.Scripts.Behaviours;
using Game.Scripts.Helpers;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField, BoxGroup("GridSize")] private int _gridSizeX;
        [SerializeField, BoxGroup("GridSize")] private int _gridSizeY;

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
        }

        private void DisposeLevel()
        {
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