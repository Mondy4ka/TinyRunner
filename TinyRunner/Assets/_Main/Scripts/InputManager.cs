using System;
using UnityEngine;

[Serializable]
public class InputManager
{
    public event Action OnClick;

    private readonly GameManager _gameManager;

    private bool _isActive;

    public InputManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void Initialize() => _gameManager.OnGameStateChanged += OnGameStateChanged;

    public void Deinitialize() => _gameManager.OnGameStateChanged -= OnGameStateChanged;

    private void OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.StartPlaying:
            case GameState.Playing:
                SetInputActive(true);
                break;
            default:
                SetInputActive(false);
                break;
        }
    }

    public void Update()
    {
        if (_isActive == false) return;

        GetClickInput();
    }

    private void SetInputActive(bool isActive) => _isActive = isActive;

    private void GetClickInput()
    {
        if (Input.GetMouseButtonDown(0) == false) return;

        OnClick?.Invoke();
    }
}