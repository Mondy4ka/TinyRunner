using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerJumper
{
    public event Action<bool> OnJumping;

    public float JumpTimer
    {
        get => _jumpTimer;
        set
        {
            _jumpTimer = Mathf.Clamp(value, 0, _jumpDuration);
            
            if (_jumpTimer >= _jumpDuration)
                IsJumping = false;
        }
    }

    public bool IsJumping
    {
        get => _isJumping;
        set
        {
            _isJumping = value;
            OnJumping?.Invoke(_isJumping);
        }
    }
    
    private readonly Transform _transform;
    private readonly AnimationCurve _animationCurve;
    private readonly float _jumpDuration;

    private readonly PlayerCollisionTracker _collisionTracker;
    private PlayerLineSwitcher _lineSwitcher;

    private float _jumpTimer;
    private bool _isJumping;
    private bool _isTopPosition;

    public PlayerJumper(Transform transform, AnimationCurve animationCurve, PlayerCollisionTracker collisionTracker)
    {
        _transform = transform;
        _animationCurve = animationCurve;
        _collisionTracker = collisionTracker;

        _jumpDuration = _animationCurve.keys.Last().time;
    }

    public void Initialize(PlayerLineSwitcher lineSwitcher)
    {
        _lineSwitcher = lineSwitcher;

        _collisionTracker.OnPlayerJump += StartJump;
        _lineSwitcher.OnLineSwitch += UpdatePosition;
    }

    public void Deinitialize()
    {
        _collisionTracker.OnPlayerJump -= StartJump;
        _lineSwitcher.OnLineSwitch -= UpdatePosition;
    }

    public void Update()
    {
        if (IsJumping == false) return;

        Jumping();
    }

    private void UpdatePosition(bool isTop) => _isTopPosition = isTop;

    private void StartJump()
    {
        if (IsJumping) return;

        IsJumping = true;
        JumpTimer = 0.0f;
    }

    private void Jumping()
    {
        JumpTimer += Time.deltaTime;

        float newPositionY = _animationCurve.Evaluate(JumpTimer);

        Vector2 newPos = _transform.position;
        newPos.y = _isTopPosition ? newPositionY : -newPositionY;

        _transform.position = newPos;
    }
}