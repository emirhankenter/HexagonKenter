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

            //Debug.Log($"Degrees: {degrees}");

            if (degrees > 330f || degrees <= 30f)
            {
                if (GetRighUpAndRightDownNeighbours(out neighbours))
                {
                    return true;
                }
            }
            else if (degrees > 30f && degrees <= 90f)
            {
                if (GetUpAndRightUpNeighbours(out neighbours))
                {
                    return true;
                }
            }
            else if (degrees > 90f && degrees <= 150f)
            {
                if (GetUpAndLeftUpNeighbours(out neighbours))
                {
                    return true;
                }
            }
            else if (degrees > 150f && degrees <= 210f)
            {
                if (GetLeftUpAndLeftDownNeighbours(out neighbours))
                {
                    return true;
                }
            }
            else if (degrees > 210f && degrees <= 270f)
            {
                if (GetDownAndLeftDownNeighbours(out neighbours))
                {
                    return true;
                }
            }
            else if (degrees > 270f && degrees <= 330f)
            {
                if (GetDownAndRightDownNeighbours(out neighbours))
                {
                    return true;
                }
            }

            if (degrees > 0 && degrees <= 180)
            {
                var closeToLeft = Math.Abs(180f - degrees) < Math.Abs(degrees - 0);

                if (closeToLeft)
                {
                    if (GetUpAndRightUpNeighbours(out neighbours))
                    {
                        return true;
                    }
                    else if (GetDownAndRightDownNeighbours(out neighbours))
                    {
                        return true;
                    }
                }
                else
                {
                    if (GetUpAndLeftUpNeighbours(out neighbours))
                    {
                        return true;
                    }
                    else if (GetDownAndLeftDownNeighbours(out neighbours))
                    {
                        return true;
                    }
                }
            }
            else
            {
                var closeToLeft = Math.Abs(360f - degrees) > Math.Abs(degrees - 180f);

                if (closeToLeft)
                {
                    if (GetLeftUpAndLeftDownNeighbours(out neighbours))
                    {
                        return true;
                    }
                    else if (GetUpAndLeftUpNeighbours(out neighbours))
                    {
                        return true;
                    }
                }
                else
                {
                    if (GetRighUpAndRightDownNeighbours(out neighbours))
                    {
                        return true;
                    }
                    else if (GetUpAndRightUpNeighbours(out neighbours))
                    {
                        return true;
                    }
                }
            }

            neighbours = (null, null);
            return false;
        }
    }
}