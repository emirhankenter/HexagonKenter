using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class HexagonLevelBehaviour : MonoBehaviour
    {
        private HexagonBehaviour[,] _hexagonBehaviours;
        public HexagonBehaviour[,] HexagonBehaviours => _hexagonBehaviours;

        protected int Width;
        protected int Height;

        public void Initiliaze(int width, int height)
        {
            Width = width;
            Height = height;

            _hexagonBehaviours = new HexagonBehaviour[width, height];
        }

        public HexagonBehaviour GetHexagon(int i, int j)
        {
            return HexagonBehaviours[i, j];
        }
    }
}
