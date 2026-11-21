using Unity.VisualScripting;
using UnityEngine;

public class PlayerCubeController : MonoBehaviour
{
    [SerializeField] private CubeController cubeController;
    [SerializeField] private CubeRotator cubeRotator;
    [SerializeField] private ObjectRotationController rotationController;
    [SerializeField] private MouseRaycastController mouseRaycastController;
    [Space]
    [SerializeField] private float freeRotationSpeed;
    [SerializeField] private float mouseRotationSpeed;
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float snapAndRotateTime;
    [SerializeField] private float errorMargin;

    private float _totalDragLength;

    private void Start()
    {
        mouseRaycastController.OnCheck.AddListener(hit =>
        {
            var hitInfo = mouseRaycastController.LastHitInfo;
            if (hit && hitInfo.collider.gameObject.TryGetComponent(out CubeElement element))
                cubeController.ChangeSelectedElement(element, hitInfo.normal);
            else if(!hit)
                cubeController.ChangeSelectedElement(null, Vector3.zero);
        });
    }
    
    public void MoveCube(Vector2Int direction, bool snapRotation)
    {
        if(snapRotation)
        {
            rotationController.SnapAndRotate(cubeRotator.CameraRelativeRotationEuler, direction, snapAndRotateTime);
        }
        else
        {
            rotationController.Rotate(direction, Time.deltaTime * freeRotationSpeed);
        }
    }

    public void MoveSegment(Vector2 direction)
    {
        if(cubeController.CurrentSelectedElement)
            cubeRotator.SetRotationByCameraDirection(direction, 0.3f);
    }

    public void MoveSelectedElement(Vector2Int direction)
    {

    }

    public void Tilt(float direction, bool snap)
    {
        rotationController.SnapAndTilt(cubeRotator.CameraRelativeRotationEuler, (int)direction, 0.3f);
    }

    public void StartRotation()
    {
        if (cubeController.CurrentSelectedElement)
        {
            _totalDragLength = 0;
            
            cubeRotator.InitializeRotation();
        }
    }

    public void ExecuteRotation(Vector2 mouseDelta)
    {
        _totalDragLength += mouseDelta.magnitude;
        if (_totalDragLength < errorMargin)
            return;
        
        cubeRotator.RotateByCameraDirection(mouseDelta, Time.deltaTime * mouseRotationSpeed);
    }

    public void EndRotation() => cubeRotator.EndRotationDrag();
}
