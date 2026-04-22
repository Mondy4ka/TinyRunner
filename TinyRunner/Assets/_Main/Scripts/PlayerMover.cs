using System;
using UnityEngine;

[Serializable]
public class PlayerMover
{
    private readonly SpeedManager _speedManager;
    private readonly Transform _transform;
    private readonly float _startXPosition;

    private float _currentSpeed;

    public PlayerMover(SpeedManager speedManager, Transform transform)
    {
        _speedManager = speedManager;
        _transform = transform;

        _startXPosition = _transform.position.x;
    }

    public void Initialize() => _speedManager.OnSpeedChanged += UpdateSpeed;

    public void Deinitialize() => _speedManager.OnSpeedChanged -= UpdateSpeed;

    public void Update() => Move();

    public void SetStartPosition() => _transform.position = new(_startXPosition, _transform.position.y);

    private void UpdateSpeed(float newSpeed)
    {
        if (newSpeed < 0) return;

        _currentSpeed = newSpeed;
    }

    private void Move()
    {
        float offset = _currentSpeed * Time.deltaTime;

        Vector2 newPosition = _transform.position;
        newPosition.x += offset;

        _transform.position = newPosition;
    }
}