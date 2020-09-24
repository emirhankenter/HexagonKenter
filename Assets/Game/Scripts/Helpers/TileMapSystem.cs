using DG.Tweening;
using Game.Scripts.Behaviours;
using Game.Scripts.Controllers;
using Mek.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Game.Scripts.Helpers
{
    public delegate void HexagonBlowDelegate(Vector2 screenPosition);
    public class TileMapSystem : MonoBehaviour
    {
        public static HexagonBlowDelegate HexagonBlowed;

        public static bool TileMapReady;

        private string _hexagonPath = "Hexagon";

        private HexagonBehaviour[,] _tileMap;

        protected int Width;
        protected int Height;

        protected float HorizontalLenght;
        protected float VerticalLenght;

        private bool _bombIsInQueue;

        public HexagonBehaviour[,] CreateHexagonTileMap(int width, int height, Transform parent)
        {
            var item = Resources.Load<HexagonBehaviour>(_hexagonPath);

            if (item == null) return null;

            GameController.BombSpawnScoreThresholdReached += OnBombSpawnInvoked;

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

            var matchingHexagons = GetMatchingHexagons();

            while (matchingHexagons.Count > 0)
            {
                ReInitializeMathing(matchingHexagons);

                matchingHexagons = GetMatchingHexagons();
            }

            return _tileMap;
        }

        private void OnDestroy()
        {
            GameController.BombSpawnScoreThresholdReached -= OnBombSpawnInvoked;
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

                             _tileMap[i, j].Hexagon.LeftUp = _tileMap[i - 1, j];

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

                            _tileMap[i, j].Hexagon.RightUp = _tileMap[i + 1, j];

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

        public void RotateAntiClockwise((HexagonBehaviour, HexagonBehaviour, HexagonBehaviour) group, Action<bool> onComplete, float stepDuration = 0.2f)
        {
            TileMapReady = false;

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

                    if (CheckMatching())
                    {
                        onComplete?.Invoke(true);
                        index = 100;
                        yield break;
                    }
                }

                TileMapReady = true;
                onComplete?.Invoke(false);
            }
        }
        public void RotateClockwise((HexagonBehaviour, HexagonBehaviour, HexagonBehaviour) group, Action<bool> onComplete, float stepDuration = 0.2f)
        {
            TileMapReady = false;

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
                                tempArray[i, j] = _tileMap[item2Indeces.i, item2Indeces.j];
                            }
                            else if (item2Indeces.i == i && item2Indeces.j == j)
                            {
                                tempArray[i, j] = _tileMap[item3Indeces.i, item3Indeces.j];
                            }
                            else if (item3Indeces.i == i && item3Indeces.j == j)
                            {
                                tempArray[i, j] = _tileMap[item1Indeces.i, item1Indeces.j];
                            }
                        }
                    }

                    _tileMap = tempArray;

                    UpdateIndexes();

                    yield return new WaitForSeconds(stepDuration);

                    if (CheckMatching())
                    {
                        onComplete?.Invoke(true);
                        index = 100;
                        yield break;
                    }
                }
                TileMapReady = true;
                onComplete?.Invoke(false);
            }
        }

        private List<HexagonBehaviour> GetMatchingHexagons()
        {
            var list = new List<HexagonBehaviour>();

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var matches = _tileMap[i, j].GetMatches();
                    if (matches.Count > 0)
                    {
                        foreach (var item in matches)
                        {
                            if (!list.Contains(item)) list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        private List<HexagonBehaviour> GetMatchingHexagons(out List<(int, int)> indexes)
        {
            var list = new List<HexagonBehaviour>();

            indexes = new List<(int, int)>();

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var matches = _tileMap[i, j].GetMatches();
                    if (matches.Count > 0)
                    {
                        if (!indexes.Contains((i, j))) indexes.Add((i, j));

                        foreach (var item in matches)
                        {
                            if (!list.Contains(item)) list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        private void ReInitializeMathing(List<HexagonBehaviour> list)
        {
            foreach (var item in list)
            {
                item.Initialize(AssetController.Instance.Colors.GetRandomElement());
            }
        }

        private bool CheckMatching()
        {
            var matchingHexagons = GetMatchingHexagons(out var indexes);

            if (matchingHexagons.Count > 0)
            {
                foreach (var index in indexes)
                {
                    _tileMap[index.Item1, index.Item2] = null;
                }

                foreach (var item in matchingHexagons)
                {
                    HexagonBlowed?.Invoke(GameController.Instance.CameraController.Camera.WorldToScreenPoint(item.transform.position));
                    Destroy(item.gameObject);
                }

                SlideHexagonsDown(indexes);

                UpdateIndexes();

                return true;
            }

            return false;
        }

        private void SlideHexagonsDown(List<(int, int)> indexes)
        {
            var dictionary = new Dictionary<int, (int, int)>(); // (first dimension, (highest second dimension, count of vertical loss))

            foreach (var index in indexes)
            {
                if (!dictionary.ContainsKey(index.Item1))
                {
                    dictionary[index.Item1] = (index.Item2, 1);
                }
                else
                {
                    if (dictionary[index.Item1].Item1 < index.Item2)
                    {
                        dictionary[index.Item1] = (index.Item2, dictionary[index.Item1].Item2 + 1);
                    }
                }
            }

            foreach (var pair in dictionary)
            {
                var list = new List<HexagonBehaviour>();

                for (int j = 0; j < Height; j++)
                {
                    if (_tileMap[pair.Key, j] != null)
                    {
                        list.Add(_tileMap[pair.Key, j]);
                    }
                }
                for (int j = 0; j < list.Count; j++)
                {
                    _tileMap[pair.Key, j] = list[j];
                    _tileMap[pair.Key, j].transform.DOMove(new Vector3(_tileMap[pair.Key, j].transform.position.x, j * HexagonBehaviour.TileYOffset + (pair.Key % 2 == 1 ? HexagonBehaviour.TileYOffset / 2 : 0)), 0.2f);
                }
            }

            SpawnHexagonsFromTop(dictionary);
        }

        private void SpawnHexagonsFromTop(Dictionary<int, (int, int)> dictionary)
        {
            var item = Resources.Load<HexagonBehaviour>(_hexagonPath);

            foreach (var pair in dictionary)
            {
                for (int j = 0; j < pair.Value.Item2; j++)
                {
                    var hexagon = Instantiate(item as HexagonBehaviour, new Vector3(HexagonBehaviour.TileXOffset * pair.Key, HexagonBehaviour.TileYOffset * Height + 1, 0), Quaternion.identity);
                    hexagon.transform.SetParent(transform, true);
                    hexagon.Initialize(_bombIsInQueue ? UnityEngine.Color.red : AssetController.Instance.Colors.GetRandomElement());
                    _tileMap[pair.Key, Height - pair.Value.Item2 + j] = hexagon;
                    hexagon.transform.DOMove(new Vector3(HexagonBehaviour.TileXOffset * pair.Key, HexagonBehaviour.TileYOffset * (Height - pair.Value.Item2 + j) + (pair.Key % 2 == 1 ? HexagonBehaviour.TileYOffset / 2 : 0), 0), 0.6f);

                    if (_bombIsInQueue)
                    {
                        Debug.Log("BombSpawned");
                        _bombIsInQueue = false;
                    }
                }
            }

            CoroutineController.DoAfterGivenTime(0.7f, () =>
            {
                UpdateIndexes();
                if (!CheckMatching())
                {
                    TileMapReady = true;
                }
            });
        }

        private void OnBombSpawnInvoked()
        {
            _bombIsInQueue = true;
        }
    }
}