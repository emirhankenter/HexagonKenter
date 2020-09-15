using Game.Scripts.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Helpers
{
    public class TileMapGenerator : MonoBehaviour
    {
        private string _hexagonPath = "Hexagon";

        public HexagonLevelBehaviour CreateHexagon(int width, int height)
        {
            var item = Resources.Load<HexagonBehaviour>(_hexagonPath);

            if (item == null) return null;

            var hexagonLevelBehaviour = new GameObject("TileMap").AddComponent<HexagonLevelBehaviour>();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var hexagon = Instantiate(item as HexagonBehaviour, hexagonLevelBehaviour.transform);

                    if (j % 2 == 0)
                    {
                        hexagon.transform.localPosition = new Vector3(i * hexagon.TileXOffset, j * hexagon.TileYOffset, 0);
                    }
                    else
                    {
                        hexagon.transform.localPosition = new Vector3(i * hexagon.TileXOffset + hexagon.TileXOffset / 2, j * hexagon.TileYOffset, 0);
                    }
                }
            }

            return hexagonLevelBehaviour;
        }
    }
}