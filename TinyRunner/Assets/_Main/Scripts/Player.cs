using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerTrigger PlayerTrigger => _playerTrigger;

    [Header("Movement Settings")]
    [SerializeField] private float _topYPosition;
    [SerializeField] private float _downYPosition;

    [Header("Hitbox Settings")]
    [SerializeField] private PlayerTrigger _playerTrigger;

    public void Initialize(GameManager gameManager, InputManager inputManager, SpeedManager speedManager)
    {
        PlayerMovement = new(gameManager, inputManager, speedManager, transform, _topYPosition, _downYPosition);

        PlayerMovement.Initialize();
    }

    private void Update() => PlayerMovement.Update();

    public void OnDestroy()
    {
        PlayerMovement.Deintialize();
    }
}