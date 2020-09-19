using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        public Camera Camera => _mainCamera;

        public Vector2 GetMouseWorldPosition()
        {
            return Camera.ScreenToWorldPoint(Input.mousePosition);
        }

        public Ray GetScreenPointToRay()
        {
            return Camera.ScreenPointToRay(Input.mousePosition);
        }
    }
}