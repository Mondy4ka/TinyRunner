using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Obstacle : MonoBehaviour // class-marker
{
    public bool IsGhost => _isGhost;

    [SerializeField] private bool _isGhost;
    [SerializeField] private ParticleSystem _breakParticle;

    public void Break()
    {
        Instantiate(_breakParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}