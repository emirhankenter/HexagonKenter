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

        private (HexagonBehaviour, HexagonBehaviour, HexagonBehaviour) _currentHexagonGroup;

        private void Awake()
        {
            InputActions = new InputActions();
            InputActions.Enable();
            InputActions.Player.Tap.performed += OnTapPerformed;
            InputActions.Player.Tap.canceled += OnTapCanceled;

            InputActions.Player.Fire.performed += OnFirePerformed;
        }

        private void OnDestroy()
        {
            InputActions.Disable();
            InputActions.Player.Tap.performed -= OnTapPerformed;
            InputActions.Player.Tap.canceled -= OnTapCanceled;

            InputActions.Player.Fire.performed -= OnFirePerformed;
        }

        private void OnTapPerformed(InputAction.CallbackContext obj)
        {

            var mousePosition = GameController.Instance.CameraController.GetMouseWorldPosition();

            if (GameController.Instance.CurrentLevel.TileMap.TryGetHexagonAtPoint(mousePosition, out HexagonBehaviour hexagon))
            {
                Deselect();

                if (hexagon.GetClosestNeighbours(mousePosition, out var neighbours))
                {
                    SelectGroup((hexagon, neighbours.Item1, neighbours.Item2));
                }
            }
        }

        private void OnTapCanceled(InputAction.CallbackContext obj)
        {
        }

        private void OnFirePerformed(InputAction.CallbackContext obj)
        {
            InputActions.Player.Fire.canceled += OnFireCanceled;
            InputActions.Player.Move.performed += OnMovePerformed;

            Debug.Log("Fire");
        }

        private void OnFireCanceled(InputAction.CallbackContext obj)
        {
            InputActions.Player.Fire.canceled -= OnFireCanceled;
            InputActions.Player.Move.performed -= OnMovePerformed;

            Debug.Log("FireCanceled");
        }

        private void OnMovePerformed(InputAction.CallbackContext obj)
        {
            var position = obj.ReadValue<Vector2>();

            //Debug.Log($"({position.x}, {position.y})");
        }

        private void SelectGroup((HexagonBehaviour, HexagonBehaviour, HexagonBehaviour) group)
        {
            _currentHexagonGroup = group;
            _currentHexagonGroup.Item1.GetComponent<SpriteRenderer>().color = Color.green;
            _currentHexagonGroup.Item2.GetComponent<SpriteRenderer>().color = Color.green;
            _currentHexagonGroup.Item3.GetComponent<SpriteRenderer>().color = Color.green;
        }

        private void Deselect()
        {
            if (_currentHexagonGroup.Item1 != null && _currentHexagonGroup.Item2 != null && _currentHexagonGroup.Item3 != null)
            {
                _currentHexagonGroup.Item1.GetComponent<SpriteRenderer>().color = Color.white;
                _currentHexagonGroup.Item2.GetComponent<SpriteRenderer>().color = Color.white;
                _currentHexagonGroup.Item3.GetComponent<SpriteRenderer>().color = Color.white;

                _currentHexagonGroup.Item1 = null;
                _currentHexagonGroup.Item2 = null;
                _currentHexagonGroup.Item3 = null;
            }
        }
    }
}