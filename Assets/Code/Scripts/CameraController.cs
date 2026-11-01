using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;

    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    public void Rotate(Vector2 delta)
    {
        transform.Rotate(_cameraTransform.up, -delta.x * speed, Space.World);
        transform.Rotate(_cameraTransform.right, delta.y * speed, Space.World);
    }
}
