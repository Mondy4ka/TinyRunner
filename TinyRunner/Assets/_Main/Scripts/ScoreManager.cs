using System;
using UnityEngine;

public class ScoreManager
{
    public event Action<int> OnScoreChanged;
    public event Action<int> OnBestScoreChanged;

    private readonly GameManager _gameManager;
    private readonly PlayerMovement _playerMovement;
    private readonly int _additionScore;

    private int _currentScore;
    private int _bestScore;

    public ScoreManager(GameManager gameManager, PlayerMovement playerMovement, int additionScore)
    {
        _gameManager = gameManager;
        _playerMovement = playerMovement;
        _additionScore = additionScore;
    }

    public void Initialize()
    {
        _gameManager.OnStartGame += ResetScore;
        _playerMovement.OnScoreDistance += AddScore;
    }

    public void Deinitialize()
    {
        _gameManager.OnStartGame -= ResetScore;
        _playerMovement.OnScoreDistance -= AddScore;
    }

    private void ResetScore()
    {
        _currentScore = 0;
        OnScoreChanged?.Invoke(_currentScore);
    }

    private void AddScore()
    {
        _currentScore += _additionScore;
        OnScoreChanged?.Invoke(_currentScore);

        if (_currentScore >  _bestScore)
        {
            SetBestScore(_currentScore);
        }
    }

    private void SetBestScore(int newBest)
    {
        if (newBest < _bestScore) return;

        _bestScore = newBest;
        OnBestScoreChanged?.Invoke(_bestScore);
    }
}
