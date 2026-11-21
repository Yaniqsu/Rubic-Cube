using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeRotator : MonoBehaviour
{
    [SerializeField] private Transform cubeRoot;
    [SerializeField] private LayerMask cubeLayer;
    [SerializeField] private Transform pivot;

    private CubeElement[] _elements;
    private CoroutineSequencer _moveSegmentSequencer;

    private int _size;
    private Vector3 _setRotationAxis;
    private Vector3 _totalRotation;
    private Vector3Int? _elementPosition;
    private Vector3 _elementNormal;

    private CubeElement[] _elementsToRotate = Array.Empty<CubeElement>();

    public Quaternion CameraRelativeRotation => CameraUtilities.GetCameraRelativeRotation(cubeRoot.localRotation);
    public Vector3 CameraRelativeRotationEuler => CameraRelativeRotation.eulerAngles;
    public bool Rotating { get; private set; }

    public event Action OnRotationEnd;
    public event Action<CubeRotationData> OnMove;

    private void Awake()
    {
        _moveSegmentSequencer = new CoroutineSequencer(this);
    }

    public void Initialize(CubeElement[] elements, int size)
    {
        _elements = elements;
        _size = size;
    }
    public void SetSelectedElement(CubeElement element)
    {
        _elementPosition = element ? element.Position : null;
    }

    public void SetSelectedElementNormal(Vector3 normal)
    {
        _elementNormal = cubeRoot.InverseTransformDirection(normal).normalized;
    }

    public void StartRotationDrag(CubeElement[] elements, int size)
    {
        _elements = elements;
        _size = size;
    }

    public void EndRotationDrag()
    {
        CleanAfterRotation();

        if (_totalRotation.magnitude >= 90)
            OnMove?.Invoke(new CubeRotationData() { elementPos = _elementPosition.Value, axis = _totalRotation });

        _totalRotation = Vector3.zero;
        OnRotationEnd?.Invoke();
    }

    public void SetRotationByCameraDirection(Vector2 direction, float time)
    {
        if (!_elementPosition.HasValue)
            return;
        
        InitializeRotation();
        
        RotateByCameraDirectionInTime(direction, 90f, time, EndRotationDrag);
    }

    public void SetRotationByAxis(Vector3Int elementPos, Vector3 axis)
    {
        ResetRotationVariables();

        _elementsToRotate = GetElementsToRotate(elementPos, axis).ToArray();

        foreach (var e in _elementsToRotate)
        {
            e.transform.SetParent(pivot);
        }

        SetRotation(axis);

        CleanAfterRotation();

        _totalRotation = Vector3.zero;
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
        _moveSegmentSequencer.AddCoroutine(RotateByAxisInTime(speed * localAxis, time), onEnd);
    }

    private IEnumerator RotateByAxisInTime(Vector3 rotationEuler, float time)
    {
        var startRotation = pivot.localRotation;
        var endRotation = Quaternion.Euler(rotationEuler);

        var elapsed = 0f;

        while(elapsed < time)
        {
            pivot.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsed / time);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        pivot.localRotation = endRotation;
    }

    private void CleanAfterRotation()
    {
        if (_elementsToRotate.Length == 0)
            return;

        if(_totalRotation.magnitude > 0)
            SnapRotation();

        foreach (var e in _elementsToRotate)
            e.transform.SetParent(cubeRoot);

        var dragRotationNormalised = _totalRotation / 90;

        CalculateNewPositions(new Vector3Int(
            Mathf.RoundToInt(dragRotationNormalised.x),
            Mathf.RoundToInt(dragRotationNormalised.y),
            Mathf.RoundToInt(dragRotationNormalised.z)));

        pivot.transform.localEulerAngles = Vector3.zero;
        _setRotationAxis = Vector3.zero;
        Rotating = false;
    }

    public void InitializeRotation()
    {
        Rotating = true;
        _totalRotation = Vector3.zero;

        ResetRotationVariables();
    }

    private void ResetRotationVariables()
    {
        _totalRotation = Vector3.zero;
        pivot.transform.localEulerAngles = Vector3.zero;
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

    private void RotateElements(Vector3 axis)
    {
        pivot.transform.Rotate(axis);
        _totalRotation += axis;
    }

    private void SetRotation(Vector3 rotationEuler)
    {
        pivot.transform.localEulerAngles = rotationEuler;

        _totalRotation = rotationEuler;
    }

    private void SnapRotation()
    {
        var rotation = _totalRotation.NormalizeVector();

        var snappedRotation = new Vector3(
            Mathf.Round(rotation.x / 90f) * 90f,
            Mathf.Round(rotation.y / 90f) * 90f,
            Mathf.Round(rotation.z / 90f) * 90f
        );

        SetRotation(snappedRotation);
    }

    private void CalculateNewPositions(Vector3Int rotation)
    {
        foreach (var e in _elementsToRotate)
        {
            e.Position = GetRotatedPosition(e.Position, rotation);
        }
    }

    private Vector3Int GetRotatedPosition(Vector3Int pos, Vector3Int rotation)
    {
        var sum = rotation.x + rotation.y + rotation.z;
        var turns = ((sum % 4) + 4) % 4;

        for (var i = 0; i < turns; i++)
        {
            if (rotation.x != 0)
                pos = new(pos.x, _size - 1 - pos.z, pos.y);
            else if (rotation.y != 0)
                pos = new(pos.z, pos.y, _size - 1 - pos.x);
            else if (rotation.z != 0)
                pos = new(_size - 1 - pos.y, pos.x, pos.z);
        }

        return pos;
    }

    private IEnumerable<CubeElement> GetElementsToRotate(Vector3Int elementPos, Vector3 rotationAxis)
    {
        foreach (var e in _elements)
        {
            if (Vector3.Dot(e.Position, rotationAxis) == Vector3.Dot(elementPos, rotationAxis))
                yield return e;
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
