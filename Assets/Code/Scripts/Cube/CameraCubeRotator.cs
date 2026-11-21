using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraCubeRotator : ACubeRotator
{
    private Vector3 _setRotationAxis;

    public Quaternion CameraRelativeRotation => CameraUtilities.GetCameraRelativeRotation(cubeRoot.localRotation);
    public Vector3 CameraRelativeRotationEuler => CameraRelativeRotation.eulerAngles;

    public void SetRotationByCameraDirection(Vector2 direction, float time)
    {
        if (!_elementPosition.HasValue)
            return;
        
        StartRotationDrag();
        
        RotateByCameraDirectionInTime(direction, 90f, time, EndRotationDrag);
    }
    public void RotateByCameraDirection(Vector2 direction, float speed)
    {
        if (!Rotating || direction.magnitude < 0.1f)
            return;

        var localAxis = GetRotationAxis(direction);

        if (_setRotationAxis == Vector3.zero)
        {
            ChooseRotationAxis(localAxis);
        }
        else if (!DirectionsMatch(_setRotationAxis, localAxis))
            return;

        RotateElements(speed * localAxis);
    }
    private void RotateByCameraDirectionInTime(Vector2 direction, float speed, float time, Action onEnd = null)
    {
        var localAxis = GetRotationAxis(direction);

        ChooseRotationAxis(localAxis);

        _totalRotation += speed * localAxis;
        StartCoroutine(RotateByAxisInTime(speed * localAxis, time, onEnd));
    }

    protected override void CleanAfterRotation()
    {
        base.CleanAfterRotation();
        
        _setRotationAxis = Vector3.zero;
    }

    private void ChooseRotationAxis(Vector3 axis)
    {
        _setRotationAxis = axis;
        _elementsToRotate = GetElementsToRotate(_elementPosition.Value, axis).ToArray();
        foreach (var e in _elementsToRotate)
        {
            e.transform.SetParent(pivot);
        }
    }

    private Vector3 GetRotationAxis(Vector2 direction)
    {
        var dragDirection = CameraUtilities.GetRotationAxisFromDirection(direction);
        var localDrag = cubeRoot.InverseTransformDirection(dragDirection).normalized;
        var rotationAxis = Vector3.Cross(_elementNormal, localDrag).normalized;
        var localAxis = transform.InverseTransformDirection(rotationAxis);
        localAxis = localAxis.GetClosestAxis();

        return localAxis;
    }

    #region UTILITIES

    private static bool DirectionsMatch(Vector3 a, Vector3 b)
    {
        if (Mathf.Abs(a.x) != Mathf.Abs(b.x))
            return false;

        if (Mathf.Abs(a.y) != Mathf.Abs(b.y))
            return false;

        if (Mathf.Abs(a.z) != Mathf.Abs(b.z))
            return false;

        return true;
    }

    #endregion
}
