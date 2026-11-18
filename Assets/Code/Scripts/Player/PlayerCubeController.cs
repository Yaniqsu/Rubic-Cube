using System.Collections;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerCubeController : MonoBehaviour
{
    [SerializeField] private CubeController cubeController;
    [SerializeField] private CubeRotator cubeRotator;
    [SerializeField] private ObjectRotationController rotationController;
    [SerializeField] private MouseRaycastController mouseRaycastController;
    [Space]
    [SerializeField] private float freeRotateSpeed;
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float snapAndRotateTime;

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

    private void Update()
    {
        //Debug.Log(cubeRotator.CameraOrientedRotationEuler);
    }

    public void MoveCube(Vector2Int direction, bool snapRotation)
    {
        if(snapRotation)
        {
            rotationController.SnapAndRotate(cubeRotator.CameraRelativeRotationEuler, direction, snapAndRotateTime);
        }
        else
        {
            rotationController.Rotate(direction, Time.deltaTime * freeRotateSpeed);
        }
    }

    public void MoveSegment(Vector2 direction)
    {
        if(cubeController.CurrentSelectedElement)
            cubeRotator.ForceRotationFromCamera(direction, 0.3f);
    }

    public void MoveSelectedElement(Vector2Int direction)
    {

    }

    public void Tilt(float direction, bool snap)
    {
        rotationController.SnapAndTilt(cubeRotator.CameraRelativeRotationEuler, (int)direction, 0.3f);
    }
}
