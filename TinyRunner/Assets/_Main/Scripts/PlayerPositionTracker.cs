using System;
using UnityEngine;

[Serializable]
public class PlayerPositionTracker
{
    public event Action OnScoringPoints;
    public event Action OnChunkSpawning;

    private readonly SpeedManager _speedManager;

    private float _currentSpeed;
    private float _traveledDistance;
    private float _traveledScore;

    public PlayerPositionTracker(SpeedManager speedManager) => _speedManager = speedManager;

    public void Initialize() => _speedManager.OnSpeedChanged += UpdateSpeed;

    public void Deinitialize() => _speedManager.OnSpeedChanged -= UpdateSpeed;

    public void Update() => Tracking();

    private void Tracking()
    {
        float offset = Time.deltaTime * _currentSpeed;

        _traveledScore += offset;
        if (_traveledScore >= 5.0f)
        {
            _traveledScore -= 5.0f;
            OnScoringPoints?.Invoke();
        }

        _traveledDistance += offset;
        if (_traveledDistance >= 20.0f)
        {
            _traveledDistance -= 20.0f;
            OnChunkSpawning?.Invoke();
        }
    }

    private void UpdateSpeed(float newSpeed)
    {
        if (newSpeed < 0) return;

        _currentSpeed = newSpeed;
    }
}