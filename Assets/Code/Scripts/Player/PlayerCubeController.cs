using UnityEngine;

public class PlayerCubeController : MonoBehaviour
{
    [SerializeField] private CubeRotator cubeRotator;
    [SerializeField] private ObjectRotationController rotationController;
    [Space]
    [SerializeField] private float freeRotateSpeed;
    [SerializeField] private float tiltSpeed;

    private void Update()
    {
        Debug.Log(cubeRotator.CameraOrientedRotationEuler);
    }

    public void MoveCube(Vector2Int direction, bool snapRotation)
    {
        rotationController.Rotate(direction, freeRotateSpeed);
    }

    public void MoveSegment(Vector2Int direction)
    {

    }

    public void MoveSelectedElement(Vector2Int direction)
    {

    }

    public void Tilt(float direction, bool snap)
    {
        if(!snap)
        {
            rotationController.Tilt(tiltSpeed * direction);
        }
    }
}
