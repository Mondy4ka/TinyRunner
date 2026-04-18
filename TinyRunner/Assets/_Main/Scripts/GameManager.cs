using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action OnStartGame;
    public event Action OnGamePaused;
    public event Action OnGameContinue;
    public event Action OnGameOver;
    public event Action OnGameRestart;

    [SerializeField] private Player _player;
    [SerializeField] private UIManager _uiManager;

    [Header("Game Speed Settings")]
    [SerializeField] private float _startSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _accelerationRate;

    [Header("Level Settings")]
    [SerializeField] private List<GameObject> _levelChunkPrefabs;
    [SerializeField] private float _chunkLength;

    [Header("Score Settings")]
    [SerializeField] private int _additionScore;

    private LevelManager _levelManager;
    private InputManager _inputManager;
    private SpeedManager _speedManager;
    private ScoreManager _scoreManager;

    private void Awake()
    {
        _inputManager = new(this);
        _speedManager = new(this, _startSpeed, _acceleration, _accelerationRate);

        _player.Initialize(this, _inputManager, _speedManager);

        _scoreManager = new(this, _player.PlayerMovement, _additionScore);
        _levelManager = new(this, _levelChunkPrefabs, _chunkLength, _player.PlayerMovement);

        _scoreManager.Initialize();
        _speedManager.Initialize();
        _inputManager.Initialize();
        _levelManager.Initialize();
        _uiManager.Initialize(this, _scoreManager);

        _player.PlayerTrigger.OnPlayerDeath += GameOver;
    }

    private void Update()
    {
        _inputManager?.Update();
        _speedManager?.Update();
    }

    public void StartGame() => OnStartGame?.Invoke();

    public void GameOver() => OnGameOver?.Invoke();
    
    public void PauseGame() => OnGamePaused?.Invoke();

    public void ContinueGame() => OnGameContinue?.Invoke();

    public void RestartGame() => OnGameRestart?.Invoke();

    private void OnDestroy()
    {
        _scoreManager?.Deinitialize();
        _inputManager?.Deinitialize();
        _speedManager?.Deinitialize();
        _levelManager?.Deinitialize();
        _uiManager.Deinitialize();

        _player.PlayerTrigger.OnPlayerDeath -= GameOver;
    }
}