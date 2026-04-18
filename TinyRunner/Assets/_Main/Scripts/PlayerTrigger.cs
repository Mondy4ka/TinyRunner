using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerTrigger : MonoBehaviour
{
    public event Action OnPlayerDeath;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.TryGetComponent(out Obstacle _))
            OnPlayerDeath?.Invoke();
    }
}