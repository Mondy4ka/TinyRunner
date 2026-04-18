using System;
using UnityEngine;

[Serializable]
public class PlayerMovement
{
    public event Action OnDistanceChanged;
    public event Action OnScoreDistance;

    private readonly GameManager _gameManager;
    private readonly InputManager _inputManager;
    private readonly SpeedManager _speedManager;
    private readonly Transform _transform;
    private readonly float _topYPosition;
    private readonly float _downYPosition;
    private readonly Vector2 _spawnPoint;

    private float _traveledDistance;
    private float _traveledScore;

    private float _currentMoveSpeed;
    private bool _isTopPosition = true;
    private bool _isMove;

    public PlayerMovement(GameManager gameManager, InputManager inputManager, SpeedManager speedManager, Transform transform, float topYPosition, float downYPosition)
    {
        _gameManager = gameManager;
        _inputManager = inputManager;
        _speedManager = speedManager;
        _transform = transform;
        _topYPosition = topYPosition;
        _downYPosition = downYPosition;
        _spawnPoint = _transform.position;
    }

    public void Initialize()
    {
        _gameManager.OnStartGame += () => SetMoveActive(true);
        _gameManager.OnGameContinue += () => SetMoveActive(true);
        _gameManager.OnGamePaused += () => SetMoveActive(false);
        _gameManager.OnGameOver += () => SetMoveActive(false);
        _gameManager.OnGameRestart += () => SetMoveActive(false);
        _gameManager.OnGameRestart += ToSpawnPosition;

        _inputManager.OnClick += SwitchPosition;
        _speedManager.OnSpeedChanged += UpdateSpeed;
    }

    public void Deintialize()
    {
        _gameManager.OnStartGame -= () => SetMoveActive(true);
        _gameManager.OnGameContinue -= () => SetMoveActive(true);
        _gameManager.OnGamePaused -= () => SetMoveActive(false);
        _gameManager.OnGameOver -= () => SetMoveActive(false);
        _gameManager.OnGameRestart -= () => SetMoveActive(false);
        _gameManager.OnGameRestart -= ToSpawnPosition;

        _inputManager.OnClick -= SwitchPosition;
        _speedManager.OnSpeedChanged -= UpdateSpeed;
    }

    public void SetMoveActive(bool isActive) => _isMove = isActive;

    public void ToSpawnPosition()
    {
        _isTopPosition = true;
        _transform.position = _spawnPoint;
    }

    public void Update()
    {
        if (_isMove == false) return;

        Move();
    }

    private void UpdateSpeed(float newSpeed)
    {
        if (newSpeed < 0) return;

        _currentMoveSpeed = newSpeed;
    }

    private void Move()
    {
        float offset = Time.deltaTime * _currentMoveSpeed;
        _traveledDistance += offset;
        _traveledScore += offset;

        if (_traveledScore >= 5.0f)
        {
            _traveledScore -= 5.0f;
            OnScoreDistance?.Invoke();
        }

        if (_traveledDistance >= 20.0f)
        {
            _traveledDistance -= 20.0f;
            OnDistanceChanged?.Invoke();
        }

        Vector2 newPosition = _transform.position;
        newPosition.x += offset;

        _transform.position = newPosition;
    }

    private void SwitchPosition()
    {
        Vector2 newPosition = _transform.position;

        if (_isTopPosition)
        {
            _isTopPosition = false;
            newPosition.y = _downYPosition;
        }
        else
        {
            _isTopPosition = true;
            newPosition.y = _topYPosition;
        }

        _transform.position = newPosition;
    }
}