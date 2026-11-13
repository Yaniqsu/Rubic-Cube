using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectRotationController : MonoBehaviour
{
    [SerializeField] private Transform objTransform;
    [SerializeField] private float speed;

    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    public void Rotate(Vector2 delta)
    {
        objTransform.Rotate(_cameraTransform.up, -delta.x * speed, Space.World);
        objTransform.Rotate(_cameraTransform.right, delta.y * speed, Space.World);
    }

    public void Rotate(Vector2 delta, float speed)
    {
        objTransform.Rotate(_cameraTransform.up, -delta.x * speed, Space.World);
        objTransform.Rotate(_cameraTransform.right, delta.y * speed, Space.World);
    }

    public void Tilt(float value)
    {
        objTransform.Rotate(_cameraTransform.forward, -value, Space.World);
    }
}