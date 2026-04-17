using UnityEngine;

public class PlayerMovement
{
    private readonly InputManager _inputManager;
    private readonly Transform _transform;
    private readonly float _topYPosition;
    private readonly float _downYPosition;
    private readonly float _startSpeed;

    private float _currentMoveSpeed;
    private bool _isTopPosition = true;
    private bool _isMove = true;

    public PlayerMovement(InputManager inputManager, Transform transform, float topYPosition, float downYPosition, float startSpeed)
    {
        _inputManager = inputManager;
        _transform = transform;
        _topYPosition = topYPosition;
        _downYPosition = downYPosition;
        _startSpeed = startSpeed;
    }

    public void Initialize()
    {
        _currentMoveSpeed = _startSpeed;

        _inputManager.OnClick += SwitchPosition;
    }

    public void Deintialize() => _inputManager.OnClick -= SwitchPosition;

    public void Update()
    {
        if (_isMove == false) return;

        Move();
    }

    private void Move()
    {
        Vector2 newPosition = _transform.position;
        newPosition.x += Time.deltaTime * _currentMoveSpeed;
        _transform.position = newPosition;
    }

    private void SwitchPosition()
    {
        Vector2 newPosition = _transform.position;

        if (_isTopPosition)
        {
            _isTopPosition = false;
            newPosition.y = _downYPosition;
        }
        else
        {
            _isTopPosition = true;
            newPosition.y = _topYPosition;
        }

        _transform.position = newPosition;
    }
}