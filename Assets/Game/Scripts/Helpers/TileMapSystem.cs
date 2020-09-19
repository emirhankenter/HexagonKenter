using Game.Scripts.Behaviours;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Helpers
{
    public class TileMapSystem : MonoBehaviour
    {
        private string _hexagonPath = "Hexagon";

        private HexagonBehaviour[,] _tileMap;

        protected int Width;
        protected int Height;

        protected float HorizontalLenght;
        protected float VerticalLenght;

        public HexagonBehaviour[,] CreateHexagonTileMap(int width, int height, Transform parent)
        {
            var item = Resources.Load<HexagonBehaviour>(_hexagonPath);

            if (item == null) return null;

            Width = width;
            Height = height;

            HorizontalLenght = HexagonBehaviour.TileXLength;
            VerticalLenght = HexagonBehaviour.TileYLegth;

            _tileMap = new HexagonBehaviour[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var hexagon = Instantiate(item as HexagonBehaviour, parent);
                    _tileMap[i, j] = hexagon;

                    hexagon.name = $"Tile ({i},{j})";

                    if (i % 2 == 0)
                    {
                        hexagon.transform.localPosition = new Vector3(i * HexagonBehaviour.TileXOffset, j * HexagonBehaviour.TileYOffset, 0);
                    }
                    else
                    {
                        hexagon.transform.localPosition = new Vector3(i * HexagonBehaviour.TileXOffset, j * HexagonBehaviour.TileYOffset + HexagonBehaviour.TileYOffset / 2, 0);
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
                                _tileMap[i, j].Hexagon.LeftDown = _tileMap[i - 1, j- 1];
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
                            _tileMap[i, j].Hexagon.RightDown = _tileMap[i + 1, j];

                            if (j < height - 1)
                            {
                                _tileMap[i, j].Hexagon.RightUp = _tileMap[i + 1, j + 1];
                            }
                        }
                        else
                        {
                            if (j > 0)
                            {
                                _tileMap[i, j].Hexagon.RightDown = _tileMap[i + 1, j - 1];
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
                    }

                    if (j < height - 1)
                    {
                        _tileMap[i, j].Hexagon.Up = _tileMap[i, j + 1];
                    }
                }
            }

            return _tileMap;
        }

        public bool IsInside(Vector2 origin, Vector2 position)
        {
            var q2x = Math.Abs(position.x - origin.x);
            var q2y = Math.Abs(position.y - origin.y);

            if (q2x > HorizontalLenght * 2 || q2y > VerticalLenght)
            {
                return false;
            }
            return VerticalLenght * 2 * HorizontalLenght - VerticalLenght * q2x - HorizontalLenght * q2y >= 0;
        }

        public bool TryGetHexagonAtPoint(Vector2 position, out HexagonBehaviour hexagonBehaviour)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (IsInside(origin: _tileMap[i, j].transform.position, position))
                    {
                        hexagonBehaviour = _tileMap[i, j];
                        return true;
                    }
                }
            }
            hexagonBehaviour = null;
            return false;
        }

        public HexagonBehaviour GetHexagonAtIndex(int i, int j)
        {
            return _tileMap[i, j];
        }
    }
}