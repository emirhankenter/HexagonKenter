using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class Hexagon<T>
    {
        public T Up;
        public T Down;

        public T LeftUp;
        public T LeftDown;

        public T RightUp;
        public T RightDown;
    }

    public class HexagonBehaviour : MonoBehaviour
    {
        public static float TileXLength => 0.6585f;
        public static float TileYLegth => 1.121f;
        public static float TileXOffset => 1.97f;
        public static float TileYOffset => 2.28f;

        [SerializeField] private SpriteRenderer _hexagonSprite;
        [SerializeField] private SpriteRenderer _outlineSprite;

        public Hexagon<HexagonBehaviour> Hexagon = new Hexagon<HexagonBehaviour>();

        public Color Color { get; private set; }

        public void Initialize(Color color)
        {
            Color = color;

            _hexagonSprite.color = color;
        }
        public void Select()
        {
            _outlineSprite.enabled = true;
        }
        public void Deselect()
        {
            _outlineSprite.enabled = false;
        }

        public bool CheckIfNeighboursCanMatch()
        {
            var dict = new Dictionary<Color, int>();

            if (Hexagon.Up != null)
            {
                if (dict.ContainsKey(Hexagon.Up.Color))
                    dict[Hexagon.Up.Color]++;
                else
                    dict[Hexagon.Up.Color] = 1;
            }
            if (Hexagon.Down != null)
            {
                if (dict.ContainsKey(Hexagon.Down.Color))
                    dict[Hexagon.Down.Color]++;
                else
                    dict[Hexagon.Down.Color] = 1;
            }
            if (Hexagon.RightUp != null)
            {
                if (dict.ContainsKey(Hexagon.RightUp.Color))
                    dict[Hexagon.RightUp.Color]++;
                else
                    dict[Hexagon.RightUp.Color] = 1;
            }
            if (Hexagon.RightDown != null)
            {
                if (dict.ContainsKey(Hexagon.RightDown.Color))
                    dict[Hexagon.RightDown.Color]++;
                else
                    dict[Hexagon.RightDown.Color] = 1;
            }
            if (Hexagon.LeftUp != null)
            {
                if (dict.ContainsKey(Hexagon.LeftUp.Color))
                    dict[Hexagon.LeftUp.Color]++;
                else
                    dict[Hexagon.LeftUp.Color] = 1;
            }
            if (Hexagon.LeftDown != null)
            {
                if (dict.ContainsKey(Hexagon.LeftDown.Color))
                    dict[Hexagon.LeftDown.Color]++;
                else
                    dict[Hexagon.LeftDown.Color] = 1;
            }

            foreach (var pair in dict)
            {
                if (pair.Value >= 3)
                {
                    return true;
                }
            }

            return false;
        }

        public List<HexagonBehaviour> GetMatches()
        {
            var list = new List<HexagonBehaviour>();

            if (GetRighUpAndRightDownNeighbours(out var neightbours1))
            {
                if (Color == neightbours1.Item1.Color && Color == neightbours1.Item2.Color)
                {
                    if (!list.Contains(this)) list.Add(this);
                    if (!list.Contains(neightbours1.Item1)) list.Add(neightbours1.Item1);
                    if (!list.Contains(neightbours1.Item2)) list.Add(neightbours1.Item2);
                }
            }
            if (GetUpAndRightUpNeighbours(out var neightbours2))
            {
                if (Color == neightbours2.Item1.Color && Color == neightbours2.Item2.Color)
                {
                    if (!list.Contains(this)) list.Add(this);
                    if (!list.Contains(neightbours2.Item1)) list.Add(neightbours2.Item1);
                    if (!list.Contains(neightbours2.Item2)) list.Add(neightbours2.Item2);
                }
            }
            if (GetUpAndLeftUpNeighbours(out var neightbours3))
            {
                if (Color == neightbours3.Item1.Color && Color == neightbours3.Item2.Color)
                {
                    if (!list.Contains(this)) list.Add(this);
                    if (!list.Contains(neightbours3.Item1)) list.Add(neightbours3.Item1);
                    if (!list.Contains(neightbours3.Item2)) list.Add(neightbours3.Item2);
                }
            }
            if (GetLeftUpAndLeftDownNeighbours(out var neightbours4))
            {
                if (Color == neightbours4.Item1.Color && Color == neightbours4.Item2.Color)
                {
                    if (!list.Contains(this)) list.Add(this);
                    if (!list.Contains(neightbours4.Item1)) list.Add(neightbours4.Item1);
                    if (!list.Contains(neightbours4.Item2)) list.Add(neightbours4.Item2);
                }
            }
            if (GetDownAndLeftDownNeighbours(out var neightbours5))
            {
                if (Color == neightbours5.Item1.Color && Color == neightbours5.Item2.Color)
                {
                    if (!list.Contains(this)) list.Add(this);
                    if (!list.Contains(neightbours5.Item1)) list.Add(neightbours5.Item1);
                    if (!list.Contains(neightbours5.Item2)) list.Add(neightbours5.Item2);
                }
            }
            if (GetDownAndRightDownNeighbours(out var neightbours6))
            {
                if (Color == neightbours6.Item1.Color && Color == neightbours6.Item2.Color)
                {
                    if (!list.Contains(this)) list.Add(this);
                    if (!list.Contains(neightbours6.Item1)) list.Add(neightbours6.Item1);
                    if (!list.Contains(neightbours6.Item2)) list.Add(neightbours6.Item2);
                }
            }

            return list;
        }

        public bool GetRighUpAndRightDownNeighbours(out (HexagonBehaviour rightDown, HexagonBehaviour rightUp) neighbours)
        {
            if (Hexagon.RightUp != null && Hexagon.RightDown != null)
            {
                neighbours.rightUp = Hexagon.RightUp;
                neighbours.rightDown = Hexagon.RightDown;

                return true;
            }

            neighbours = (null, null);
            return false;
        }

        public bool GetUpAndRightUpNeighbours(out (HexagonBehaviour rightUp, HexagonBehaviour up) neighbours)
        {
            if (Hexagon.Up != null && Hexagon.RightUp != null)
            {
                neighbours.up = Hexagon.Up;
                neighbours.rightUp = Hexagon.RightUp;

                return true;
            }

            neighbours = (null,null);
            return false;
        }

        public bool GetUpAndLeftUpNeighbours(out (HexagonBehaviour up, HexagonBehaviour leftUp) neighbours)
        {
            if (Hexagon.Up != null && Hexagon.LeftUp != null)
            {
                neighbours.up = Hexagon.Up;
                neighbours.leftUp = Hexagon.LeftUp;

                return true;
            }

            neighbours = (null, null);
            return false;
        }

        public bool GetLeftUpAndLeftDownNeighbours(out (HexagonBehaviour leftUp, HexagonBehaviour leftDown) neighbours)
        {
            if (Hexagon.LeftUp != null && Hexagon.LeftDown != null)
            {
                neighbours.leftUp = Hexagon.LeftUp;
                neighbours.leftDown = Hexagon.LeftDown;

                return true;
            }

            neighbours = (null, null);
            return false;
        }

        public bool GetDownAndLeftDownNeighbours(out (HexagonBehaviour leftDown, HexagonBehaviour down) neighbours)
        {
            if (Hexagon.LeftDown != null && Hexagon.Down != null)
            {
                neighbours.down = Hexagon.Down;
                neighbours.leftDown = Hexagon.LeftDown;

                return true;
            }

            neighbours = (null, null);
            return false;
        }

        public bool GetDownAndRightDownNeighbours(out (HexagonBehaviour down, HexagonBehaviour rightDown) neighbours)
        {
            if (Hexagon.RightDown != null && Hexagon.Down != null)
            {
                neighbours.down = Hexagon.Down;
                neighbours.rightDown = Hexagon.RightDown;

                return true;
            }

            neighbours = (null, null);
            return false;
        }

        public bool GetClosestNeighbours(Vector2 targetPosition, out (HexagonBehaviour, HexagonBehaviour) neighbours)
        {
            var difference = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
            var radians = Math.Atan(difference.y / difference.x);
            if (1 / difference.x < 0) radians += Math.PI;
            if (1 / radians < 0) radians += 2 * Math.PI;
            var degrees = radians * 180f / Math.PI;

            if (degrees > 330f || degrees <= 30f)
            {
                if (GetRighUpAndRightDownNeighbours(out neighbours)) return true;
            }
            else if (degrees > 30f && degrees <= 90f)
            {
                if (GetUpAndRightUpNeighbours(out neighbours)) return true;
            }
            else if (degrees > 90f && degrees <= 150f)
            {
                if (GetUpAndLeftUpNeighbours(out neighbours)) return true;
            }
            else if (degrees > 150f && degrees <= 210f)
            {
                if (GetLeftUpAndLeftDownNeighbours(out neighbours)) return true;
            }
            else if (degrees > 210f && degrees <= 270f)
            {
                if (GetDownAndLeftDownNeighbours(out neighbours)) return true;
            }
            else if (degrees > 270f && degrees <= 330f)
            {
                if (GetDownAndRightDownNeighbours(out neighbours)) return true;
            }
            neighbours = (null, null);
            return false;
        }
    }
}