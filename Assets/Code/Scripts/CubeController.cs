using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private int size;
    [SerializeField] private int randomizeTurns;
    [SerializeField] private CubeSpawner spawner;
    [SerializeField] private Transform cubeRoot;
    [SerializeField] private LayerMask cubeLayer;
    [SerializeField] private Transform pivot;
    
    private CubeElement[] _elements;
    private Camera _main;
    
    private bool _rotating;
    private float _dragLength;
    private Vector3 _setRotationAxis;
    private CubeElement _selectedCubeElement;
    private Vector3Int _elementPosition;
    private Vector3 _elementFaceNormal;
    private Vector3 _totalDragRotation;

    private CubeElement[] _elementsToRotate;

    private void Start()
    {
        _main = Camera.main;
        _elements = spawner.SpawnCube(size).ToArray();
        Randomize(randomizeTurns);
    }

    private void Randomize(int turns)
    {
        for(var i = 0; i < turns; i++)
        {
            var rotation = Random.Range(1, 4) * (Random.Range(0, 2) == 0 ? 1 : -1) * 90;

            switch(Random.Range(0, 3))
            {
                case 0:
                    ForceRotation(rotation * Vector3.right);
                    break;

                case 1:
                    ForceRotation(rotation * Vector3.up);
                    break;

                case 2:
                    ForceRotation(rotation * Vector3.forward);
                    break;
            }
        }
    }

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
            foreach(var e  in _elementsToRotate)
            {
                e.transform.SetParent(pivot);
            }
        }
        else if (!DirectionsMatch(_setRotationAxis, localAxis))
            return;

        RotateElements(localAxis);
    }

    private void ForceRotation(Vector3 axis)
    {
        _totalDragRotation = Vector3.zero;
        
        _elementsToRotate = GetElementsToRotate(axis).ToArray();

        foreach (var e in _elementsToRotate)
        {
            e.transform.SetParent(pivot);
        }

        RotateElements(axis);

        EndRotation();
    }

    private void RotateElements(Vector3 axis)
    {
        pivot.transform.Rotate(axis);
        _totalDragRotation += axis;
    }

    public void EndRotation()
    {
        SnapRotation();

        foreach (var e in _elementsToRotate)
            e.transform.SetParent(cubeRoot);

        pivot.transform.localEulerAngles = Vector3.zero;

        var dragRotationNormalised = _totalDragRotation / 90;
        CalculateNewPositions(new Vector3Int(
            Mathf.RoundToInt(dragRotationNormalised.x), 
            Mathf.RoundToInt(dragRotationNormalised.y),
            Mathf.RoundToInt(dragRotationNormalised.z)));

        if (CubeSolved())
            Debug.Log("Solved");

        _dragLength = 0;
        _setRotationAxis = Vector3.zero;
        _totalDragRotation = Vector3.zero;
        _rotating = false;
        _selectedCubeElement = null;
    }

    private void InitializeRotation(RaycastHit hitInfo)
    {
        hitInfo.collider.gameObject.TryGetComponent(out _selectedCubeElement);
        _elementPosition = _selectedCubeElement.Position;
        _elementFaceNormal = cubeRoot.InverseTransformDirection(hitInfo.normal).normalized;
        _rotating = true;
    }

    private void SnapRotation()
    {
        var rotation = pivot.localEulerAngles;
        
        var rest = new Vector3(
            Mathf.Abs(rotation.x) % 90,
            Mathf.Abs(rotation.y) % 90,
            Mathf.Abs(rotation.z) % 90);

        if(rest.magnitude > 45)
            RotateElements((AbsSign(rest) * 90) - rest);
        else
            RotateElements(-rest);  

    }

    private void CalculateNewPositions(Vector3Int rotation)
    {
        foreach(var e in _elementsToRotate)
        {
            e.Position = GetRotatedPosition(e.Position, rotation);
        }
    }

    private Vector3Int GetRotatedPosition(Vector3Int pos, Vector3Int rotation)
    {
        var sum = rotation.x + rotation.y + rotation.z;
        var turns = ((sum % 4) + 4) % 4;

        for(var i = 0; i < turns; i++)
        {
            if (rotation.x != 0)
                pos = new(pos.x, size - 1 - pos.z, pos.y);
            else if (rotation.y != 0)
                pos = new(pos.z, pos.y, size - 1 - pos.x);
            else if (rotation.z != 0)
                pos = new(size - 1 - pos.y, pos.x, pos.z);
        }

        return pos;
    }

    private Vector3 AbsSign(Vector3 vec)
    {
        return new(
            vec.x == 0 ? 0 : 1,
            vec.y == 0 ? 0 : 1,
            vec.z == 0 ? 0 : 1);
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

    private IEnumerable<CubeElement> GetElementsToRotate(Vector3 rotationAxis)
    {
        foreach (var e in _elements)
        {
            if(Vector3.Dot(e.Position, rotationAxis) == Vector3.Dot(_elementPosition, rotationAxis))
                yield return e;
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

    private bool CubeSolved()
    {
        foreach(var e in _elements)
        {
            if (e.Position != e.OriginalPosition)
                return false;
        }

        return true;
    }
}
