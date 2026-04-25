using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerCollisionTracker PlayerCollisionTracker => _collisionTracker;
    public PlayerMover PlayerMover { get; private set; }
    public PlayerPositionTracker PlayerPositionTracker { get; private set; }
    public PlayerLineSwitcher PlayerLineSwitcher { get; private set; }
    public PlayerJumper PlayerJumper { get; private set; }


    [Header("Movement Settings")]
    [SerializeField] private float _topYPosition;
    [SerializeField] private float _downYPosition;

    [Header("Hitbox Settings")]
    [SerializeField] private PlayerCollisionTracker _collisionTracker;

    [Header("Animation Settings")]
    [SerializeField] private AnimationCurve _jumpCurve;
    [SerializeField] private Ease _ease;
    [SerializeField] private float _animtaionDuration;
    [SerializeField] private ParticleSystem _trail;

    private GameManager _gameManager;
    private bool _isMoving;

    public void Initialize(GameManager gameManager, InputManager inputManager, SpeedManager speedManager)
    {
        PlayerMover = new(speedManager, transform);
        PlayerPositionTracker = new(speedManager);
        PlayerJumper = new(transform, _jumpCurve, _collisionTracker);
        PlayerLineSwitcher = new(inputManager, transform, _topYPosition, _downYPosition, _ease, _animtaionDuration, PlayerJumper);

        PlayerMover.Initialize();
        PlayerPositionTracker.Initialize();
        PlayerLineSwitcher.Initialize();
        PlayerJumper.Initialize(PlayerLineSwitcher);

        _gameManager = gameManager;
        _gameManager.OnGameStateChanged += OnGameStateChanged;
    }

    public void Deinitialize()
    {
        _gameManager.OnGameStateChanged -= OnGameStateChanged;

        PlayerMover.Deinitialize();
        PlayerPositionTracker.Deinitialize();
        PlayerJumper.Deinitialize();
        PlayerLineSwitcher.Deinitialize();
    }

    private void OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Paused:
            case GameState.GameOver:
                _isMoving = false;
                _trail.Stop();
                break;
            case GameState.Menu:
                PlayerMover.SetStartPosition();
                PlayerLineSwitcher.SetStartPosition();
                _isMoving = false;
                _trail.Stop();
                break;
            case GameState.StartPlaying:
            case GameState.Playing:
                _trail.Play();
                _isMoving = true;
                break;
        }
    }

    private void Update()
    {
        if (_isMoving == false) return;

        PlayerMover?.Update();
        PlayerJumper?.Update();
        PlayerPositionTracker?.Update();
    }

    private void OnDestroy() => Deinitialize();
}