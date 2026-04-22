using System;
using UnityEngine;

[Serializable]
public class SpeedManager
{
    public event Action<float> OnSpeedChanged;

    private readonly GameManager _gameManager;
    private readonly float _startSpeed;
    private readonly float _acceleration;
    private readonly float _accelerationRate;

    private float _currentTime;
    private float _currentSpeed;
    private bool _isActive;

    public SpeedManager(GameManager gameManager, float startSpeed, float acceleration, float accelerationRate)
    {
        _gameManager = gameManager;
        _startSpeed = startSpeed;
        _acceleration = acceleration;
        _accelerationRate = accelerationRate;
    }

    public void Initialize() => _gameManager.OnGameStateChanged += OnGameStateChanged;

    public void Deinitialize() => _gameManager.OnGameStateChanged -= OnGameStateChanged;

    private void OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Menu:
            case GameState.Paused:
                SetActive(false);
                break;
            case GameState.GameOver:
                ResetSpeed();
                break;
            case GameState.StartPlaying:
                StartUpdate();
                break;
            case GameState.Playing:
                SetActive(true);
                break;
        }
    }

    public void Update()
    {
        if (_isActive == false) return;

        UpdateSpeed();
    }

    private void SetActive(bool isActive) => _isActive = isActive;

    private void StartUpdate()
    {
        ResetSpeed();
        SetActive(true);
    }

    private void ResetSpeed()
    {
        _currentTime = 0.0f;
        _currentSpeed = _startSpeed;

        OnSpeedChanged?.Invoke(_currentSpeed);
    }

    private void UpdateSpeed()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime >= 60.0f / _accelerationRate)
        {
            _currentTime = 0.0f;
            _currentSpeed += _acceleration;
            OnSpeedChanged?.Invoke(_currentSpeed);
        }
    }
}