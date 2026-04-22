using System;

public class ScoreManager
{
    public event Action<int> OnScoreChanged;
    public event Action<int> OnBestScoreChanged;

    private readonly GameManager _gameManager;
    private readonly PlayerPositionTracker _positionTracker;
    private readonly int _additionScore;

    private int _currentScore;
    private int _bestScore;

    public ScoreManager(GameManager gameManager, PlayerPositionTracker positionTracker, int additionScore)
    {
        _gameManager = gameManager;
        _positionTracker = positionTracker;
        _additionScore = additionScore;
    }

    public void Initialize()
    {
        _gameManager.OnGameStateChanged += OnGameStateChanged;
        _positionTracker.OnScoringPoints += AddScore;
    }

    public void Deinitialize()
    {
        _gameManager.OnGameStateChanged -= OnGameStateChanged;
        _positionTracker.OnScoringPoints -= AddScore;
    }

    private void OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.StartPlaying:
                ResetScore();
                break;
        }
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