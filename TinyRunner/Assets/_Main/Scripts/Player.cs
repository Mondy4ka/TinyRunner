using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement PlayerMovement {  get; private set; }

    [Header("Movement Settings")]
    [SerializeField] private float _startSpeed;
    [SerializeField] private float _topYPosition;
    [SerializeField] private float _downYPosition;

    public void Initialize(InputManager inputManager)
    {
        PlayerMovement = new(inputManager, transform, _topYPosition, _downYPosition, _startSpeed);
    
        PlayerMovement.Initialize();
    }

    private void Update() => PlayerMovement.Update();

    public void OnDestroy()
    {
        PlayerMovement.Deintialize();
    }
}