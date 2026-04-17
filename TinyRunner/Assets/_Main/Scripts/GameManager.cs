using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action OnStartGame;
    public event Action OnGamePaused;
    public event Action OnGameOver;

    [SerializeField] private Player _player;

    [Header("Game Speed Settings")]
    [SerializeField] private float _startSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _accelerationRate;

    private InputManager _inputManager;
    private SpeedManager _speedManager;

    private void Awake()
    {
        _inputManager = new(this);
        _speedManager = new(this, _startSpeed, _acceleration, _accelerationRate);

        _player.Initialize(_inputManager);
    }

    private void Update()
    {
        _inputManager?.Update();
        _speedManager?.Update();
    }

    public void StartGame()
    {
        OnStartGame?.Invoke();
    }

    private void OnDestroy()
    {
        _inputManager?.Deinitialize();
        _speedManager.Deinitialize();
    }
}