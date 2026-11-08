using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeRotator : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float errorMargin;
    [SerializeField] private Transform cubeRoot;
    [SerializeField] private LayerMask cubeLayer;
    [SerializeField] private Transform pivot;

    private CubeElement[] _elements;
    private Camera _main;

    private int _size;
    private bool _rotating;
    private float _dragLength;
    private Vector3 _setRotationAxis;
    private CubeElement _selectedCubeElement;
    private Vector3Int _elementPosition;
    private Vector3 _elementFaceNormal;
    private Vector3 _totalRotation;

    private CubeElement[] _elementsToRotate;

    public event Action onRotationEnd;
    public event Action onMove;

    private void Awake()
    {
        _main = Camera.main;
    }

    public void Initialize(CubeElement[] elements, int size)
    {
        _elements = elements;
        _size = size;
    }

    #region INTERFACE

    public void StartRotation()
    {
        var ray = _main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics.Raycast(ray, out var hitInfo, float.PositiveInfinity, cubeLayer);

        if (hit)
        {
            InitializeRotation(hitInfo);
        }
    }

    public void ExecuteRotation(Vector2 mouseDelta)
    {
        if (!_rotating || mouseDelta.magnitude < 0.1f)
            return;

        _dragLength += mouseDelta.magnitude;

        if (_dragLength < errorMargin)
            return;

        var dragDirection = _main.transform.right * mouseDelta.x + _main.transform.up * mouseDelta.y;
        var localDrag = cubeRoot.InverseTransformDirection(dragDirection).normalized;
        var rotationAxis = Vector3.Cross(_elementFaceNormal, localDrag).normalized;
        var localAxis = transform.InverseTransformDirection(rotationAxis);
        localAxis = GetClosestAxis(localAxis);

        if (_setRotationAxis == Vector3.zero)
        {
            ChooseRotationAxis(localAxis);
        }
        else if (!DirectionsMatch(_setRotationAxis, localAxis))
            return;

        RotateElements(Time.deltaTime * speed * localAxis);
    }

    public void EndRotation()
    {
        if (_totalRotation.magnitude > 90)
            onMove?.Invoke();
        
        CleanAfterRotation();

        onRotationEnd?.Invoke();
    }

    public void ForceRotation(Vector3Int elementPos, Vector3 axis)
    {
        ResetRotationVariables();

        _elementsToRotate = GetElementsToRotate(elementPos, axis).ToArray();

        foreach (var e in _elementsToRotate)
        {
            e.transform.SetParent(pivot);
        }

        RotateElements(axis);

        CleanAfterRotation();
    }

    #endregion

    private void CleanAfterRotation()
    {
        SnapRotation();

        foreach (var e in _elementsToRotate)
            e.transform.SetParent(cubeRoot);

        var dragRotationNormalised = _totalRotation / 90;

        CalculateNewPositions(new Vector3Int(
            Mathf.RoundToInt(dragRotationNormalised.x),
            Mathf.RoundToInt(dragRotationNormalised.y),
            Mathf.RoundToInt(dragRotationNormalised.z)));

        pivot.transform.localEulerAngles = Vector3.zero;
        _dragLength = 0;
        _setRotationAxis = Vector3.zero;
        _rotating = false;
        _selectedCubeElement = null;
    }

    private void InitializeRotation(RaycastHit hitInfo)
    {
        hitInfo.collider.gameObject.TryGetComponent(out _selectedCubeElement);
        _elementPosition = _selectedCubeElement.Position;
        _elementFaceNormal = cubeRoot.InverseTransformDirection(hitInfo.normal).normalized;
        _rotating = true;

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
        _elementsToRotate = GetElementsToRotate(_elementPosition, axis).ToArray();
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
        var rotation = NormalizeVector(_totalRotation);

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

    #region UTILITIES

    private static Vector3 NormalizeVector(Vector3 vector)
    {
        return new Vector3(
            vector.x > 180f ? vector.x - 360f : vector.x,
            vector.y > 180f ? vector.y - 360f : vector.y,
            vector.z > 180f ? vector.z - 360f : vector.z);
    }

    private static Vector3 AbsSign(Vector3 vec)
    {
        return new(
            vec.x == 0 ? 0 : 1,
            vec.y == 0 ? 0 : 1,
            vec.z == 0 ? 0 : 1);
    }

    private static Vector3 GetClosestAxis(Vector3 axis)
    {
        var abs = new Vector3(
            Mathf.Abs(axis.x),
            Mathf.Abs(axis.y),
            Mathf.Abs(axis.z)
        );

        if (abs.x > abs.y && abs.x > abs.z)
            return Vector3.right * Mathf.Sign(axis.x);

        if (abs.y > abs.x && abs.y > abs.z)
            return Vector3.up * Mathf.Sign(axis.y);

        return Vector3.forward * Mathf.Sign(axis.z);
    }

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
