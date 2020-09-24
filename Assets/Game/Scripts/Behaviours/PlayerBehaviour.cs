using Game.Scripts.Controllers;
using Game.Scripts.Helpers;
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
        public static event Action PlayerMoveOccured;
        public static event Action<Direction> Rotated;

        [SerializeField] private LayerMask _targetLayer;

        public InputActions InputActions;

        private (HexagonBehaviour, HexagonBehaviour, HexagonBehaviour) _currentHexagonGroup;

        private bool _canRotate = false;

        private void Awake()
        {
            InputActions = new InputActions();
            InputActions.Enable();

            GameController.GameStarted += OnGameStarted;
            GameController.GameOver += OnGameOver;
        }

        private void OnDestroy()
        {
            InputActions.Disable();

            GameController.GameStarted -= OnGameStarted;
            GameController.GameOver -= OnGameOver;
        }

        public void OnGameStarted()
        {
            InputActions.Enable();

            InputActions.Player.Tap.performed += OnTapPerformed;
            InputActions.Player.Tap.canceled += OnTapCanceled;

            InputActions.Player.Fire.performed += OnFirePerformed;

            Rotated += OnRotated;
        }

        private void OnGameOver()
        {
            InputActions.Disable();

            InputActions.Player.Tap.performed -= OnTapPerformed;
            InputActions.Player.Tap.canceled -= OnTapCanceled;

            InputActions.Player.Fire.performed -= OnFirePerformed;

            Rotated -= OnRotated;

            Deselect();

            if (CoroutineController.IsCoroutineRunning("WaitTileMapReadyRoutineKey"))
            {
                CoroutineController.StopThisCoroutine("WaitTileMapReadyRoutineKey");
            }
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
            switch (direction)
            {
                case Direction.Up:
                    break;
                case Direction.Down:
                    break;
                case Direction.Right:
                    GameController.Instance.CurrentLevel.TileMap.RotateAntiClockwise(_currentHexagonGroup, OnRotateCompleted, 0.2f); ;
                    break;
                case Direction.Left:
                    GameController.Instance.CurrentLevel.TileMap.RotateClockwise(_currentHexagonGroup, OnRotateCompleted, 0.2f);
                    break;
                default:
                    break;
            }

            CoroutineController.StartCoroutine("WaitTileMapReadyRoutineKey", WaitTileMapReadyRoutine());

            void OnRotateCompleted(bool state)
            {
                if (state)
                {
                    Deselect();
                }
            }

            PlayerMoveOccured?.Invoke();
        }

        private IEnumerator WaitTileMapReadyRoutine()
        {
            InputActions.Player.Tap.performed -= OnTapPerformed;
            InputActions.Player.Tap.canceled -= OnTapCanceled;

            while (!TileMapSystem.TileMapReady)
            {
                yield return null;
            }

            InputActions.Player.Tap.performed += OnTapPerformed;
            InputActions.Player.Tap.canceled += OnTapCanceled;
        }
    }
}