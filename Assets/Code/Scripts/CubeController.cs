using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private CubeSpawner spawner;
    [SerializeField] private Transform cubeRoot;
    [SerializeField] private LayerMask cubeLayer;
    
    private Dictionary<Vector3Int, CubeElement> _elements;
    private Camera _main;
    
    private bool _rotating;
    private float _dragLength;
    private CubeElement _selectedCubeElement;
    private Vector3Int _elementPosition;
    private Vector3 _elementFaceNormal;

    private void Start()
    {
        _main = Camera.main;
        _elements = spawner.SpawnCube(3);
    }

    public void StartRotation()
    {
        var ray = _main.ScreenPointToRay(Input.mousePosition);
        var hit = Physics.Raycast(ray, out var hitInfo, float.PositiveInfinity, cubeLayer);

        if (hit)
        {
            hitInfo.collider.gameObject.TryGetComponent(out _selectedCubeElement);
            _elementPosition = GetCurrentElementPosition();
            _elementFaceNormal = hitInfo.normal;
            _dragLength = 0;
            _rotating = true;
        }
    }

    public void ExecuteRotation(Vector2 mouseDelta)
    {
        if (!_rotating || mouseDelta.magnitude < 0.1f)
            return;

        _dragLength += mouseDelta.magnitude;

        if (_dragLength < 0.5f)
            return;
        
        var dragDirection = _main.transform.right * mouseDelta.x + _main.transform.up * mouseDelta.y;
        var rotationAxis = Vector3.Cross(_elementFaceNormal, dragDirection).normalized;

        var localAxis = cubeRoot.InverseTransformDirection(rotationAxis);
        localAxis = GetClosestAxis(localAxis);

        var pieces = GetElementsToRotate(localAxis);
        var centre = GetCentre(pieces.ToArray());

        foreach(var p in pieces)
        {
            p.transform.RotateAround(centre, localAxis, 1);
        }
    }

    public void EndRotation()
    {
        _rotating = false;
        _selectedCubeElement = null;
    }

    Vector3 GetClosestAxis(Vector3 axis)
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

    private Vector3Int GetCurrentElementPosition()
    {
        foreach (var (pos, elem) in _elements)
        {
            if (elem == _selectedCubeElement)
                return pos;
        }

        return Vector3Int.zero;
    }

    private IEnumerable<CubeElement> GetElementsToRotate(Vector3 rotationAxis)
    {
        foreach (var (pos, elem) in _elements)
        {
            if (rotationAxis.x != 0 && pos.x == _elementPosition.x)
                yield return elem;

            if (rotationAxis.y != 0 && pos.y == _elementPosition.y)
                yield return elem;

            if (rotationAxis.z != 0 && pos.z == _elementPosition.z)
                yield return elem;
        }
    }

    private Vector3 GetCentre(CubeElement[] elements)
    {
        var centre = Vector3.zero;

        foreach(var e in elements)
        {
            centre += e.transform.localPosition;
        }

        centre /= elements.Length;

        return centre;
    }
}
