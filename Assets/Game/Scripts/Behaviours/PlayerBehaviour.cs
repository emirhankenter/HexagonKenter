using Game.Scripts.Controllers;
using Game.Scripts.Models;
using Mek.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Behaviours
{
    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }
    public class PlayerBehaviour : MonoBehaviour
    {
        public static event Action<Direction> Rotated;

        [SerializeField] private LayerMask _targetLayer;

        public InputActions InputActions;

        private (HexagonBehaviour, HexagonBehaviour, HexagonBehaviour) _currentHexagonGroup;

        private bool _canRotate = false;

        private void Awake()
        {
            InputActions = new InputActions();
            InputActions.Enable();
            InputActions.Player.Tap.performed += OnTapPerformed;
            InputActions.Player.Tap.canceled += OnTapCanceled;

            InputActions.Player.Fire.performed += OnFirePerformed;

            Rotated += OnRotated;
        }

        private void OnDestroy()
        {
            InputActions.Disable();
            InputActions.Player.Tap.performed -= OnTapPerformed;
            InputActions.Player.Tap.canceled -= OnTapCanceled;

            InputActions.Player.Fire.performed -= OnFirePerformed;

            Rotated -= OnRotated;
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
                    _canRotate = true;

                    Debug.Log($"NeighboursCanMatch: {hexagon.CheckIfNeighboursCanMatch()}");
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
        }

        private void OnFireCanceled(InputAction.CallbackContext obj)
        {
            InputActions.Player.Fire.canceled -= OnFireCanceled;
            InputActions.Player.Move.performed -= OnMovePerformed;
        }

        private void OnMovePerformed(InputAction.CallbackContext obj)
        {
            var position = obj.ReadValue<Vector2>();

            if (!_canRotate) return;

            if (position.x >= 2)
            {
                Rotated?.Invoke(Direction.Right);
                InputActions.Player.Move.performed -= OnMovePerformed;
                return;
            }

            if (position.x <= -2)
            {
                Rotated?.Invoke(Direction.Left);
                InputActions.Player.Move.performed -= OnMovePerformed;
                return;
            }

            Debug.Log($"({position.x}, {position.y})");
        }

        private void SelectGroup((HexagonBehaviour, HexagonBehaviour, HexagonBehaviour) group)
        {
            _currentHexagonGroup = group;

            _currentHexagonGroup.Item1.Select();
            _currentHexagonGroup.Item2.Select();
            _currentHexagonGroup.Item3.Select();
        }

        private void Deselect()
        {
            if (_currentHexagonGroup.Item1 != null && _currentHexagonGroup.Item2 != null && _currentHexagonGroup.Item3 != null)
            {
                _currentHexagonGroup.Item1.Deselect();
                _currentHexagonGroup.Item2.Deselect();
                _currentHexagonGroup.Item3.Deselect();

                _currentHexagonGroup.Item1 = null;
                _currentHexagonGroup.Item2 = null;
                _currentHexagonGroup.Item3 = null;
            }
        }

        private void OnRotated(Direction direction)
        {
            _canRotate = false;
            InputActions.Player.Tap.performed -= OnTapPerformed;
            InputActions.Player.Tap.canceled -= OnTapCanceled;

            switch (direction)
            {
                case Direction.Up:
                    break;
                case Direction.Down:
                    break;
                case Direction.Right:
                    GameController.Instance.CurrentLevel.TileMap.RotateAntiClockwise(_currentHexagonGroup, 0.2f);
                    break;
                case Direction.Left:
                    GameController.Instance.CurrentLevel.TileMap.RotateClockwise(_currentHexagonGroup, 0.2f);
                    break;
                default:
                    break;
            }

            CoroutineController.DoAfterGivenTime(0.2f, () => 
            {
                _canRotate = true;
                InputActions.Player.Tap.performed += OnTapPerformed;
                InputActions.Player.Tap.canceled += OnTapCanceled;
            });
        }
    }
}