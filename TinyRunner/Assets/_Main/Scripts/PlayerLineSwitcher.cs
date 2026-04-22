using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class PlayerLineSwitcher
{
    public event Action<bool> OnLineSwitch;

    private readonly InputManager _inputManager;
    private readonly PlayerJumper _jumper;
    private readonly Transform _transform;
    private readonly float _topYPosition;
    private readonly float _downYPosition;
    private readonly float _startYPosition;
    private readonly Ease _ease;
    private readonly float _animationDuration;

    private bool _isTopPosition = true;
    private bool _isBlocked = false;

    public PlayerLineSwitcher(InputManager inputManager, Transform transform, float topYPosition, float downYPosition, Ease ease, float animationDuration, PlayerJumper jumper)
    {
        _inputManager = inputManager;
        _transform = transform;
        _topYPosition = topYPosition;
        _downYPosition = downYPosition;
        _ease = ease;
        _animationDuration = animationDuration;
        _jumper = jumper;

        _startYPosition = _transform.position.y;
    }

    public void Initialize()
    {
        _inputManager.OnClick += SwitchPosition;
        _jumper.OnJumping += SetBlock;
    }

    public void Deinitialize()
    {
        _inputManager.OnClick -= SwitchPosition;
        _jumper.OnJumping -= SetBlock;
    }

    public void SetStartPosition()
    {
        _isTopPosition = true;
        _transform.position = new(_transform.position.x, _startYPosition);
    }

    public void SetBlock(bool isBlock) => _isBlocked = isBlock;

    private void SwitchPosition()
    {
        if (_isBlocked) return;

        SetBlock(true);

        float newPositionY = _isTopPosition ? _downYPosition : _topYPosition;
        
        _isTopPosition = !_isTopPosition;
        OnLineSwitch?.Invoke(_isTopPosition);

        _transform.DOMoveY(newPositionY, _animationDuration)
            .SetEase(_ease)
            .OnComplete(() => _isBlocked = false);
    }
}