using System;
using UnityEngine;

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

    public void Intiailize()
    {
        SetActive(false);
        ResetSpeed();

        _gameManager.OnStartGame += StartUpdate;
    }

    public void Deinitialize() => _gameManager.OnStartGame -= StartUpdate;
    
    public void SetActive(bool isActive) => _isActive = isActive;
    
    public  void Update()
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
    }

    private void UpdateSpeed()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime >= 60.0f / _accelerationRate)
        {
            _currentSpeed += _acceleration;
            OnSpeedChanged?.Invoke(_currentSpeed);
        }
    }
}