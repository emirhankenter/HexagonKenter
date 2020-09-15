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
        [SerializeField] private TileMapGenerator _gridController;

        [SerializeField, BoxGroup("GridSize")] private int _gridSizeX;
        [SerializeField, BoxGroup("GridSize")] private int _gridSizeY;

        private void Awake()
        {
            PrepareLevel();
        }

        private void PrepareLevel()
        {
            CreateHexagon();
        }

        public void CreateHexagon()
        {
            _gridController.CreateHexagon(_gridSizeX, _gridSizeY);
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