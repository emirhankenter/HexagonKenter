using DG.Tweening;
using Game.Scripts.Behaviours;
using Game.Scripts.Controllers;
using System;
using System.Collections;
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

                    hexagon.Initialize(AssetController.Instance.Colors.GetRandomElement());

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

            UpdateIndexes();

            return _tileMap;
        }

        private void UpdateIndexes()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    _tileMap[i, j].name = $"Tile ({i},{j})";
                    _tileMap[i, j].Hexagon = new Hexagon<HexagonBehaviour>();
                    if (i > 0)
                    {
                        if (i % 2 == 1)
                        {
                            _tileMap[i, j].Hexagon.LeftDown = _tileMap[i - 1, j];

                            if (j < Height - 1)
                            {
                                _tileMap[i, j].Hexagon.LeftUp = _tileMap[i - 1, j + 1];
                            }
                        }
                        else
                        {
                            if (j > 0)
                            {
                                _tileMap[i, j].Hexagon.LeftDown = _tileMap[i - 1, j - 1];
                            }

                            if (j < Height - 1)
                            {
                                _tileMap[i, j].Hexagon.LeftUp = _tileMap[i - 1, j];
                            }
                        }
                    }

                    if (i < Width - 1)
                    {
                        if (i % 2 == 1)
                        {
                            _tileMap[i, j].Hexagon.RightDown = _tileMap[i + 1, j];

                            if (j < Height - 1)
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

                            if (j < Height - 1)
                            {
                                _tileMap[i, j].Hexagon.RightUp = _tileMap[i + 1, j];
                            }
                        }
                    }

                    if (j > 0)
                    {
                        _tileMap[i, j].Hexagon.Down = _tileMap[i, j - 1];
                    }

                    if (j < Height - 1)
                    {
                        _tileMap[i, j].Hexagon.Up = _tileMap[i, j + 1];
                    }
                }
            }
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

        public (int i, int j) GetHexagonIndexes(HexagonBehaviour hexagon)
        {
            return _tileMap.FindIndex(hexagon);
        }

        public void RotateAntiClockwise((HexagonBehaviour, HexagonBehaviour, HexagonBehaviour) group, float stepDuration = 0.2f,  Action onComplete = null)
        {
            StartCoroutine(Rotate());
            IEnumerator Rotate()
            {
                var index = 0;

                while (index < 3)
                {
                    index++;

                    var tempItem1 = group.Item1;
                    var tempItem2 = group.Item2;
                    var tempItem3 = group.Item3;

                    HexagonBehaviour[,] tempArray;

                    tempArray = (HexagonBehaviour[,])_tileMap.Clone();

                    var temp1Positions = group.Item1.transform.position;
                    var temp2Positions = group.Item2.transform.position;
                    var temp3Positions = group.Item3.transform.position;

                    group.Item1.transform.DOMove(temp2Positions, stepDuration);
                    group.Item2.transform.DOMove(temp3Positions, stepDuration);
                    group.Item3.transform.DOMove(temp1Positions, stepDuration);

                    var item1Indeces = _tileMap.FindIndex(group.Item1);
                    var item2Indeces = _tileMap.FindIndex(group.Item2);
                    var item3Indeces = _tileMap.FindIndex(group.Item3);

                    for (int i = 0; i < Width; i++)
                    {
                        for (int j = 0; j < Height; j++)
                        {
                            if (item1Indeces.i == i && item1Indeces.j == j)
                            {
                                tempArray[i, j] = _tileMap[item3Indeces.i, item3Indeces.j];
                            }
                            else if (item2Indeces.i == i && item2Indeces.j == j)
                            {
                                tempArray[i, j] = _tileMap[item1Indeces.i, item1Indeces.j];
                            }
                            else if (item3Indeces.i == i && item3Indeces.j == j)
                            {
                                tempArray[i, j] = _tileMap[item2Indeces.i, item2Indeces.j];
                            }
                        }
                    }

                    _tileMap = tempArray;

                    UpdateIndexes();

                    yield return new WaitForSeconds(stepDuration);
                }

                onComplete?.Invoke();
            }
        }
        public void RotateClockwise((HexagonBehaviour, HexagonBehaviour, HexagonBehaviour) group, float stepDuration = 0.2f, Action onComplete = null)
        {
            StartCoroutine(Rotate());
            IEnumerator Rotate()
            {
                var index = 0;

                while (index < 3)
                {
                    index++;

                    var tempItem1 = group.Item1;
                    var tempItem2 = group.Item2;
                    var tempItem3 = group.Item3;

                    HexagonBehaviour[,] tempArray;

                    tempArray = (HexagonBehaviour[,])_tileMap.Clone();

                    var temp1Positions = group.Item1.transform.position;
                    var temp2Positions = group.Item2.transform.position;
                    var temp3Positions = group.Item3.transform.position;

                    group.Item1.transform.DOMove(temp3Positions, stepDuration);
                    group.Item2.transform.DOMove(temp1Positions, stepDuration);
                    group.Item3.transform.DOMove(temp2Positions, stepDuration);

                    var item1Indeces = _tileMap.FindIndex(group.Item1);
                    var item2Indeces = _tileMap.FindIndex(group.Item2);
                    var item3Indeces = _tileMap.FindIndex(group.Item3);

                    for (int i = 0; i < Width; i++)
                    {
                        for (int j = 0; j < Height; j++)
                        {
                            if (item1Indeces.i == i && item1Indeces.j == j)
                            {
                                tempArray[i, j] = _tileMap[item3Indeces.i, item3Indeces.j];
                            }
                            else if (item2Indeces.i == i && item2Indeces.j == j)
                            {
                                tempArray[i, j] = _tileMap[item1Indeces.i, item1Indeces.j];
                            }
                            else if (item3Indeces.i == i && item3Indeces.j == j)
                            {
                                tempArray[i, j] = _tileMap[item2Indeces.i, item2Indeces.j];
                            }
                        }
                    }

                    _tileMap = tempArray;

                    UpdateIndexes();

                    yield return new WaitForSeconds(stepDuration);
                }

                onComplete?.Invoke();
            }
        }
    }
}