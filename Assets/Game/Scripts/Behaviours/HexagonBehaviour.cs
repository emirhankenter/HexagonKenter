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
    }
}