using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _offsetX;

    private void Update() => Follow();

    private void Follow()
    {
        Vector3 newPosition = _cameraTransform.position;
        newPosition.x = _target.position.x + _offsetX;

        _cameraTransform.position = newPosition;
    }

    private void OnValidate()
    {
        if (_target != null && _cameraTransform != null)
            Follow();
    }
}