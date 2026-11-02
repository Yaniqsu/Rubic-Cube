using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private CubeSpawner spawner;
    [SerializeField] private Transform cubeRoot;
    [SerializeField] private LayerMask cubeLayer;
    [SerializeField] private Transform pivot;
    
    private Dictionary<Vector3Int, CubeElement> _elements;
    private Camera _main;
    
    private bool _rotating;
    private float _dragLength;
    private Vector3 _setRotationAxis;
    private CubeElement _selectedCubeElement;
    private Vector3Int _elementPosition;
    private Vector3 _elementFaceNormal;
    private Vector3 _totalDragRotation;

    private CubeElement[] _elementsToRotate;
    private Vector3 _rotationCentre;

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
            _elementFaceNormal = cubeRoot.InverseTransformDirection(hitInfo.normal).normalized;
            _dragLength = 0;
            _setRotationAxis = Vector3.zero;
            _totalDragRotation = Vector3.zero;
            _rotating = true;
        }
    }

    public void ExecuteRotation(Vector2 mouseDelta)
    {
        if (!_rotating || mouseDelta.magnitude < 0.1f)
            return;

        _dragLength += mouseDelta.magnitude;

        if (_dragLength < 1)
            return;
        
        var dragDirection = _main.transform.right * mouseDelta.x + _main.transform.up * mouseDelta.y;

        var localDrag = cubeRoot.InverseTransformDirection(dragDirection).normalized;

        var rotationAxis = Vector3.Cross(_elementFaceNormal, localDrag).normalized;

        var localAxis = transform.InverseTransformDirection(rotationAxis);
        localAxis = GetClosestAxis(localAxis);

        if (_setRotationAxis == Vector3.zero)
        {
            _setRotationAxis = localAxis;
            _elementsToRotate = GetElementsToRotate(localAxis).ToArray();
            _rotationCentre = GetCentre(_elementsToRotate);
            foreach(var e  in _elementsToRotate)
            {
                e.transform.SetParent(pivot);
            }
        }
        else if (!DirectionsMatch(_setRotationAxis, localAxis))
            return;

        RotateElements(localAxis);
    }
    private void RotateElements(Vector3 axis)
    {
        pivot.transform.Rotate(axis);
        _totalDragRotation += axis;
    }

    public void EndRotation()
    {
        _rotating = false;
        _selectedCubeElement = null;
        SnapRotation();

        foreach (var e in _elementsToRotate)
            e.transform.SetParent(cubeRoot);

        pivot.transform.localEulerAngles = Vector3.zero;
    }


    private void SnapRotation()
    {
        var rest = new Vector3(
            _totalDragRotation.x % 800,
            _totalDragRotation.y % 800,
            _totalDragRotation.z % 800);
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
            if(Vector3.Dot(pos, rotationAxis) == Vector3.Dot(_elementPosition, rotationAxis))
                yield return elem;
        }
    }

    private Vector3 GetCentre(CubeElement[] elements)
    {
        var centre = Vector3.zero;

        foreach(var e in elements)
        {
            centre += e.transform.position;
        }

        centre /= elements.Length;

        return centre;
    }

    private bool DirectionsMatch(Vector3 a, Vector3 b)
    {
        if (Mathf.Abs(a.x) != Mathf.Abs(b.x))
            return false;

        if (Mathf.Abs(a.y) != Mathf.Abs(b.y))
            return false;

        if (Mathf.Abs(a.z) != Mathf.Abs(b.z))
            return false;

        return true;
    }
}
