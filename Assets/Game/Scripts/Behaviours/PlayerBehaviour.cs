using Game.Scripts.Controllers;
using Game.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Behaviours
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private LayerMask _targetLayer;

        public InputActions InputActions;

        private void Awake()
        {
            InputActions = new InputActions();
            InputActions.Enable();
            InputActions.Player.Fire.performed += OnFirePerformed;
            InputActions.Player.Fire.canceled += OnFireCanceled;
        }

        private void OnDestroy()
        {
            InputActions.Disable();
            InputActions.Player.Fire.performed -= OnFirePerformed;
            InputActions.Player.Fire.canceled -= OnFireCanceled;
        }

        private void OnFirePerformed(InputAction.CallbackContext obj)
        {
            InputActions.Player.Move.performed += OnMovePerformed;

            if (GameController.Instance.CurrentLevel.TileMap.TryGetHexagonAtPoint(GameController.Instance.CameraController.GetMouseWorldPosition(), out HexagonBehaviour hexagon))
            {
                Debug.Log($"{hexagon.name}");
            }
        }

        private void OnFireCanceled(InputAction.CallbackContext obj)
        {
            InputActions.Player.Move.performed -= OnMovePerformed;
        }

        private void OnMovePerformed(InputAction.CallbackContext obj)
        {
            var position = obj.ReadValue<Vector2>();
        }
    }
}