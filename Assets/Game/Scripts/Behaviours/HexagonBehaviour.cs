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
        public float TileXOffset => 1.97f;
        public float TileYOffset => 2.28f;

        public Hexagon<HexagonBehaviour> Hexagon = new Hexagon<HexagonBehaviour>();

        public (HexagonBehaviour, HexagonBehaviour, HexagonBehaviour) GetGroup()
        {
            return (this, Hexagon.Up, Hexagon.RightUp);
        }
    }
}