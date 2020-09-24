using Game.Scripts.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static event Action<int> ScoreChanged;
    private static int _currentScore;
    public static int CurrentScore
    {
        get { return _currentScore; }
        set
        {
            _currentScore = value;
            ScoreChanged?.Invoke(_currentScore);
            if (_currentScore > 0 && _currentScore % GameController.BombSpawnThreshold == 0)
            {
                GameController.Instance.BombSpawnScoreThresholdReached?.Invoke();
            }
        }
    }
}
