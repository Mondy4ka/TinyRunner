using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerCollisionTracker : MonoBehaviour
{
    public event Action OnPlayerDeath;
    public event Action OnPlayerJump;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.TryGetComponent(out Obstacle _))
            OnPlayerDeath?.Invoke();

        if (collision.TryGetComponent(out JumpPad _))
            OnPlayerJump?.Invoke();
    }
}