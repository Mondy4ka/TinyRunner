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

    public void Initialize()
    {
        SetActive(false);
        ResetSpeed();

        _gameManager.OnStartGame += StartUpdate;
        _gameManager.OnGameOver += ResetSpeed;
        _gameManager.OnGamePaused += () => SetActive(false);
        _gameManager.OnGameContinue += () => SetActive(true);
    }

    public void Deinitialize()
    {
        _gameManager.OnStartGame -= StartUpdate;
        _gameManager.OnGameOver -= ResetSpeed;
        _gameManager.OnGamePaused -= () => SetActive(false);
        _gameManager.OnGameContinue -= () => SetActive(true);
    }

    public void SetActive(bool isActive) => _isActive = isActive;

    public void Update()
    {
        if (_isActive == false) return;

        UpdateSpeed();
    }

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