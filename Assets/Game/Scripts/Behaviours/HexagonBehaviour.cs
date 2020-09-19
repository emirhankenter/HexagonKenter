using System;
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

        public Hexagon<HexagonBehaviour> Hexagon = new Hexagon<HexagonBehaviour>();

        public (HexagonBehaviour, HexagonBehaviour, HexagonBehaviour) GetGroup()
        {
            return (this, Hexagon.Up, Hexagon.RightUp);
        }

        public Hexagon<HexagonBehaviour> GetNeighbours()
        {
            return Hexagon;
        }

        public bool GetRightUpNeighbours(out (HexagonBehaviour up, HexagonBehaviour rightUp) neighbours)
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

        public bool GetClosestNeighbours(Vector2 targetPosition, out (HexagonBehaviour, HexagonBehaviour) neighbours)
        {
            var difference = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
            var radians = Math.Atan(difference.y / difference.x);
            if (1 / difference.x < 0) radians += Math.PI;
            if (1 / radians < 0) radians += 2 * Math.PI;
            var degrees = radians * 180f / Math.PI;

            if (degrees > 330f || degrees <= 30f)
            {
                if (Hexagon.RightUp != null && Hexagon.RightDown != null)
                {
                    neighbours.Item1 = Hexagon.RightUp;
                    neighbours.Item2 = Hexagon.RightDown;

                    return true;
                }
            }
            else if (degrees > 30f && degrees <= 90f)
            {
                if (Hexagon.Up != null && Hexagon.RightUp != null)
                {
                    neighbours.Item1 = Hexagon.Up;
                    neighbours.Item2 = Hexagon.RightUp;

                    return true;
                }
            }
            else if (degrees > 90f && degrees <= 150f)
            {
                if (Hexagon.Up != null && Hexagon.LeftUp != null)
                {
                    neighbours.Item1 = Hexagon.Up;
                    neighbours.Item2 = Hexagon.LeftUp;

                    return true;
                }
            }
            else if (degrees > 150f && degrees <= 210f)
            {
                if (Hexagon.LeftUp != null && Hexagon.LeftDown != null)
                {
                    neighbours.Item1 = Hexagon.LeftUp;
                    neighbours.Item2 = Hexagon.LeftDown;

                    return true;
                }
            }
            else if (degrees > 210f && degrees <= 270f)
            {
                if (Hexagon.LeftDown != null && Hexagon.Down != null)
                {
                    neighbours.Item1 = Hexagon.LeftDown;
                    neighbours.Item2 = Hexagon.Down;

                    return true;
                }
            }
            else if (degrees > 270f && degrees <= 330f)
            {
                if (Hexagon.Down != null && Hexagon.RightDown != null)
                {
                    neighbours.Item1 = Hexagon.Down;
                    neighbours.Item2 = Hexagon.RightDown;

                    return true;
                }
            }

            if (degrees > 0 && degrees <= 180)
            {
                var closeToLeft = Math.Abs(180f - degrees) < Math.Abs(degrees - 0);

                if (closeToLeft)
                {
                    if (Hexagon.Up != null && Hexagon.RightUp != null)
                    {
                        neighbours.Item1 = Hexagon.Up;
                        neighbours.Item2 = Hexagon.RightUp;

                        return true;
                    }
                    else if (Hexagon.Down != null && Hexagon.RightDown != null)
                    {
                        neighbours.Item1 = Hexagon.Down;
                        neighbours.Item2 = Hexagon.RightDown;

                        return true;
                    }
                }
                else
                {
                    if (Hexagon.Up != null && Hexagon.LeftUp != null)
                    {
                        neighbours.Item1 = Hexagon.Up;
                        neighbours.Item2 = Hexagon.LeftUp;

                        return true;
                    }
                    else if (Hexagon.LeftDown != null && Hexagon.Down != null)
                    {
                        neighbours.Item1 = Hexagon.LeftDown;
                        neighbours.Item2 = Hexagon.Down;

                        return true;
                    }
                }
            }
            else
            {
                var closeToLeft = Math.Abs(360f - degrees) > Math.Abs(degrees - 180f);

                if (closeToLeft)
                {
                    if (Hexagon.LeftUp != null && Hexagon.LeftDown != null)
                    {
                        neighbours.Item1 = Hexagon.LeftUp;
                        neighbours.Item2 = Hexagon.LeftDown;

                        return true;
                    }
                    else if (Hexagon.Up != null && Hexagon.LeftUp != null)
                    {
                        neighbours.Item1 = Hexagon.Up;
                        neighbours.Item2 = Hexagon.LeftUp;

                        return true;
                    }
                }
                else
                {
                    if (Hexagon.RightUp != null && Hexagon.RightDown != null)
                    {
                        neighbours.Item1 = Hexagon.RightUp;
                        neighbours.Item2 = Hexagon.RightDown;

                        return true;
                    }
                    else if (Hexagon.Up != null && Hexagon.RightUp != null)
                    {
                        neighbours.Item1 = Hexagon.Up;
                        neighbours.Item2 = Hexagon.RightUp;

                        return true;
                    }
                }
            }

            neighbours = (null, null);
            return false;
        }
    }
}