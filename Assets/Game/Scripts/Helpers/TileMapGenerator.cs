using Game.Scripts.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Helpers
{
    public class TileMapGenerator : MonoBehaviour
    {
        private string _hexagonPath = "Hexagon";

        public void CreateHexagon(int width, int height)
        {
            var item = Resources.Load<HexagonBehaviour>(_hexagonPath);

            if (item == null) return;

            var parent = new GameObject("TiliMap");

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var hexagon = Instantiate(item as HexagonBehaviour, parent.transform);

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
            
        }
    }
}