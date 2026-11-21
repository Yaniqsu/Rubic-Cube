using System.Collections;
using UnityEngine;

public class ObjectRotationController : MonoBehaviour
{
    [SerializeField] private Transform objTransform;

    private Transform _cameraTransform;

    public Vector3 RotationEuler => objTransform.localEulerAngles;

    private Coroutine _snapAndRotateRoutine;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    public void Rotate(Vector2 delta, float speed)
    {
        objTransform.Rotate(GetRotateVector(speed * delta), Space.World);
    }

    public void SnapAndRotate(Vector3 rotation, Vector2Int direction, float time)
    {
        if (_snapAndRotateRoutine != null)
            StopCoroutine(_snapAndRotateRoutine);
        
        _snapAndRotateRoutine = StartCoroutine(SnapAndRotateRoutine(rotation, direction, time));
    }

    public void SnapAndTilt(Vector3 rotation, int direction, float time)
    {
        if (_snapAndRotateRoutine != null)
            StopCoroutine(_snapAndRotateRoutine);

        _snapAndRotateRoutine = StartCoroutine(SnapAndTiltRoutine(rotation, -direction, time));
    }

    public void Tilt(float value)
    {
        objTransform.Rotate(_cameraTransform.forward, -value, Space.World);
    }

    public void SetRotation(Vector3 rotationEuler)
    {
        objTransform.localEulerAngles = GetSetVector(rotationEuler);
    }

    public void SetRotation(Quaternion rotation)
    {
        objTransform.localRotation = rotation;
    }

    private Vector3 GetSetVector(Vector3 inVector)
    {
        return inVector;
    }

    private Vector3 GetRotateVector(Vector2 inVector)
    {
        return -inVector.x * _cameraTransform.up + inVector.y * _cameraTransform.right;
    }

    private IEnumerator SnapAndRotateRoutine(Vector3 rotation, Vector2Int direction, float time)
    {
        var snapped = Quaternion.Euler(rotation.GetSnappedRotation(90));
        var offset = Quaternion.AngleAxis(90, new Vector3(direction.y, -direction.x));
        var targetRotation = offset * snapped;

        yield return HandleRotation(targetRotation, time);
    }

    private IEnumerator SnapAndTiltRoutine(Vector3 rotation, int direction, float time)
    {
        var snapped = Quaternion.Euler(rotation.GetSnappedRotation(90));
        var offset = Quaternion.AngleAxis(direction * 90, Vector3.forward);
        var targetRotation = offset * snapped;

        yield return HandleRotation(targetRotation, time);
    }

    private IEnumerator HandleRotation(Quaternion endRotation, float time)
    {
        var startQuaternion = objTransform.localRotation;
        var endQuaternion = endRotation.GetOriginalRotation(_cameraTransform);
        var elapsed = 0f;

        while (elapsed < time)
        {
            SetRotation(Quaternion.Slerp(startQuaternion, endQuaternion, elapsed / time));

            elapsed += Time.deltaTime;
            yield return null;
        }

        SetRotation(Quaternion.Slerp(startQuaternion, endQuaternion, 1));
    }
}