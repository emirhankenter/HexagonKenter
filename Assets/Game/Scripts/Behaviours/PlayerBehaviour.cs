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

        private HexagonBehaviour _currentHexagon;

        private void Awake()
        {
            InputActions = new InputActions();
            InputActions.Enable();
            InputActions.Player.Tap.performed += OnTapPerformed;
            InputActions.Player.Tap.canceled += OnTapCanceled;
        }

        private void OnDestroy()
        {
            InputActions.Disable();
            InputActions.Player.Tap.performed -= OnTapPerformed;
            InputActions.Player.Tap.canceled -= OnTapCanceled;
        }

        private void OnTapPerformed(InputAction.CallbackContext obj)
        {
            InputActions.Player.Fire.performed += OnFirePerformed;
            InputActions.Player.Fire.canceled += OnFireCanceled;

            if (GameController.Instance.CurrentLevel.TileMap.TryGetHexagonAtPoint(GameController.Instance.CameraController.GetMouseWorldPosition(), out HexagonBehaviour hexagon))
            {
                Deselect();
                Select(hexagon);
            }
        }

        private void OnTapCanceled(InputAction.CallbackContext obj)
        {
        }

        private void OnFirePerformed(InputAction.CallbackContext obj)
        {
            InputActions.Player.Move.performed += OnMovePerformed;
        }

        private void OnFireCanceled(InputAction.CallbackContext obj)
        {
            InputActions.Player.Move.performed -= OnMovePerformed;
            InputActions.Player.Fire.performed -= OnFireCanceled;
        }

        private void OnMovePerformed(InputAction.CallbackContext obj)
        {
            var position = obj.ReadValue<Vector2>();
        }

        private void Select(HexagonBehaviour hexagon)
        {
            _currentHexagon = hexagon;
            _currentHexagon.GetComponent<SpriteRenderer>().color = Color.green;
        }

        private void Deselect()
        {
            if (_currentHexagon != null)
            {
                _currentHexagon.GetComponent<SpriteRenderer>().color = Color.white;
                _currentHexagon = null;
            }
        }
    }
}