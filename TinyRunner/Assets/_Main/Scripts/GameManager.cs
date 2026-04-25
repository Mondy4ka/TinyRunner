using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChanged;

    //public event Action OnStartGame;
    //public event Action OnGamePaused;
    //public event Action OnGameContinue;
    //public event Action OnGameOver;
    //public event Action OnGameRestart;

    public GameState GameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            OnGameStateChanged?.Invoke(_gameState);
        }
    }

    [SerializeField] private PlayerController _player;
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

    private GameState _gameState;

    private void Awake()
    {
        _inputManager = new(this);
        _speedManager = new(this, _startSpeed, _acceleration, _accelerationRate);

        _player.Initialize(this, _inputManager, _speedManager);

        _scoreManager = new(this, _player.PlayerPositionTracker, _additionScore);
        _levelManager = new(this, _levelChunkPrefabs, _chunkLength, _player.PlayerPositionTracker);

        _scoreManager.Initialize();
        _speedManager.Initialize();
        _inputManager.Initialize();
        _levelManager.Initialize();
        _uiManager.Initialize(this, _scoreManager);

        _player.PlayerCollisionTracker.OnPlayerDeath += GameOver;
    }

    private void Start()
    {
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
        //Application.targetFrameRate = 999;
        QualitySettings.vSyncCount = 0;

        GameState = GameState.Menu;
    }

    private void Update()
    {
        _inputManager?.Update();
        _speedManager?.Update();
    }

    public void StartGame() => GameState = GameState.StartPlaying;

    public void GameOver() => GameState = GameState.GameOver;

    public void PauseGame() => GameState = GameState.Paused;

    public void ContinueGame() => GameState = GameState.Playing;

    public void RestartGame() => GameState = GameState.Menu;

    private void OnDestroy()
    {
        _scoreManager?.Deinitialize();
        _inputManager?.Deinitialize();
        _speedManager?.Deinitialize();
        _levelManager?.Deinitialize();
        _uiManager.Deinitialize();

        _player.PlayerCollisionTracker.OnPlayerDeath -= GameOver;
    }
}

public enum GameState
{
    Menu,
    StartPlaying,
    Playing,
    Paused,
    GameOver
}