using Game.Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class HexagonLevelBehaviour : MonoBehaviour
    {
        private HexagonBehaviour[,] _hexagonBehaviours;
        public HexagonBehaviour[,] HexagonBehaviours => _hexagonBehaviours;

        public TileMapSystem TileMap;

        protected int Width;
        protected int Height;

        public void Initiliaze(int width, int height)
        {
            Width = width;
            Height = height;

            TileMap = gameObject.AddComponent<TileMapSystem>();

            _hexagonBehaviours = new HexagonBehaviour[width, height];

            _hexagonBehaviours = TileMap.CreateHexagonTileMap(width, height, transform);
        }
    }
}
