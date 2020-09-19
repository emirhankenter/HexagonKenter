using Game.Scripts.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Helpers
{
    public class TileMapSystem : MonoBehaviour
    {
        private string _hexagonPath = "Hexagon";

        private HexagonBehaviour[,] _tileMap;

        public HexagonLevelBehaviour CreateHexagonTileMap(int width, int height)
        {
            var item = Resources.Load<HexagonBehaviour>(_hexagonPath);

            if (item == null) return null;

            var hexagonLevelBehaviour = new GameObject("TileMap").AddComponent<HexagonLevelBehaviour>();
            hexagonLevelBehaviour.Initiliaze(width, height);

            _tileMap = new HexagonBehaviour[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var hexagon = Instantiate(item as HexagonBehaviour, hexagonLevelBehaviour.transform);
                    _tileMap[i, j] = hexagon;

                    hexagon.name = $"Tile ({i},{j})";
                    hexagonLevelBehaviour.HexagonBehaviours[i, j] = hexagon;

                    if (i % 2 == 0)
                    {
                        hexagon.transform.localPosition = new Vector3(i * hexagon.TileXOffset, j * hexagon.TileYOffset, 0);
                    }
                    else
                    {
                        hexagon.transform.localPosition = new Vector3(i * hexagon.TileXOffset, j * hexagon.TileYOffset + hexagon.TileYOffset / 2, 0);
                    }

                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (i > 0)
                    {
                        if (i % 2 == 1)
                        {
                            _tileMap[i,j].Hexagon.LeftDown = _tileMap[i - 1, j];

                            if (j < height - 1)
                            {
                                _tileMap[i, j].Hexagon.LeftUp = _tileMap[i - 1, j + 1];
                            }
                        }
                        else
                        {
                            if (j > 0)
                            {
                                _tileMap[i, j].Hexagon.LeftDown = _tileMap[i - 1, j];
                            }

                            if (j < height - 1)
                            {
                                _tileMap[i, j].Hexagon.LeftUp = _tileMap[i - 1, j];
                            }
                        }
                    }

                    if (i < width - 1)
                    {
                        if (i % 2 == 1)
                        {
                            if (j > 0)
                            {
                                _tileMap[i, j].Hexagon.RightDown = _tileMap[i + 1, j - 1];
                            }

                            if (j < height - 1)
                            {
                                _tileMap[i, j].Hexagon.RightUp = _tileMap[i + 1, j + 1];
                            }
                        }
                        else
                        {
                            if (j > 0)
                            {
                                _tileMap[i, j].Hexagon.RightDown = _tileMap[i + 1, j];
                            }

                            if (j < height - 1)
                            {
                                _tileMap[i, j].Hexagon.RightUp = _tileMap[i + 1, j];
                            }
                        }
                    }

                    if (j > 0)
                    {
                        _tileMap[i, j].Hexagon.Down = _tileMap[i, j - 1];

                        if (j < height - 1)
                        {
                            _tileMap[i, j].Hexagon.Up = _tileMap[i, j + 1];
                        }
                    }
                    else
                    {
                        if (j < height - 1)
                        {
                            _tileMap[i, j].Hexagon.Up = _tileMap[i, j + 1];
                        }
                    }

                }
            }

            return hexagonLevelBehaviour;
        }
    }
}