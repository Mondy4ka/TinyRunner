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

    public void Initialize()
    {
        _gameManager.OnStartGame += () => SetInputActive(true);
        _gameManager.OnGamePaused += () => SetInputActive(false);
        _gameManager.OnGameOver += () => SetInputActive(false);
        _gameManager.OnGameContinue += () => SetInputActive(true);
    }

    public void Deinitialize()
    {
        _gameManager.OnStartGame -= () => SetInputActive(true);
        _gameManager.OnGamePaused -= () => SetInputActive(false);
        _gameManager.OnGameOver -= () => SetInputActive(false);
        _gameManager.OnGameContinue -= () => SetInputActive(true);
    }

    public void Update()
    {
        if (_isActive == false) return;

        GetClickInput();
    }

    public void SetInputActive(bool isActive) => _isActive = isActive;

    private void GetClickInput()
    {
        if (Input.GetMouseButtonDown(0) == false) return;

        OnClick?.Invoke();
    }
}