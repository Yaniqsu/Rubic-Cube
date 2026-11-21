using System.Linq;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class CubeController : MonoBehaviour
{
    [SerializeField] private CubeSpawner spawner;
    [SerializeField] private CameraCubeRotator rotator;
    [SerializeField] private Transform cubeRoot;
    [SerializeField] private int defaultSize;
    [SerializeField] private int randomizeTurns;

    private CubeElement[] _elements;
    private readonly Stack<CubeRotationData> _rotationHistory = new();
    private CubeElement _selectedElement;

    public int Size { get; private set; }
    public bool HasSelectedElement => _selectedElement;
    public Vector3Int? SelectedElementPosition => _selectedElement ? _selectedElement.Position : null;
    public Vector3 ElementFaceNormal { get; private set; }

    public event Action onCubeSolved;
    public event Action onMove;
    public event Action onUndo;

    public void Initialize(int randomizedTurns)
    {
        Size = GameRequest.cubeSize;
        if (Size == 0) Size = defaultSize;
        
        _elements = spawner.SpawnCube(Size).ToArray();

        rotator.Initialize(_elements, Size);
        rotator.OnMove += OnMove;
        if (randomizedTurns > 0) rotator.OnRotationEnd += OnRotationEnd;
        
        Randomize(randomizedTurns == 0 ? randomizeTurns : randomizedTurns);
    }

    public void Back()
    {
        if (_rotationHistory.Count == 0)
            return;

        var data = _rotationHistory.Pop();

        rotator.SetRotationByAxis(data.elementPos, -data.axis);
        onUndo?.Invoke();
    }

    private void OnRotationEnd()
    {
        if (IsCubeSolved())
            OnCubeSolved();
    }

    private void Randomize(int turns)
    {
        var axises = new List<Vector3>(new[] { Vector3.right, Vector3.up, Vector3.forward });
        
        for (var i = 0; i < turns; i++)
        {
            var elementPos = new Vector3Int(
                Random.Range(0, Size),
                Random.Range(0, Size),
                Random.Range(0, Size));

            var rotation = Random.Range(1, 4) * (Random.Range(0, 2) == 0 ? 1 : -1) * 90;
            var maxRange = i == 0 ? 3 : 2;
            var axis = axises[Random.Range(0, maxRange)];
            axises.Remove(axis);
            axises.Add(axis);

            rotator.SetRotationByAxis(elementPos, rotation * axis);
        }
    }

    private bool IsCubeSolved()
    {
        foreach (var e in _elements)
        {
            if (e.Position != e.OriginalPosition)
                return false;
        }

        return true;
    }

    private void OnCubeSolved()
    {
        ChangeSelectedElement(null, Vector3.zero);
        onCubeSolved?.Invoke();
    }

    private void OnMove(CubeRotationData rotationData)
    {
        _rotationHistory.Push(rotationData);
        
        onMove?.Invoke();
    }

    public void ChangeSelectedElement(CubeElement element, Vector3 normal)
    {
        if (rotator.Rotating)
            return;
        
        if(_selectedElement)
            _selectedElement.ToggleHighlight(false);

        _selectedElement = element;
        ElementFaceNormal = normal;

        rotator.SetSelectedElement(_selectedElement);
        rotator.SetSelectedElementNormal(ElementFaceNormal);

        if(_selectedElement)
            _selectedElement.ToggleHighlight(true);
    }

    public void SetSelectedElement(Vector3Int position)
    {
        if (PositionValid(position))
        {
            ChangeSelectedElement(GetElement(position), GetFacingFaceNormal());
        }
    }

    public void MoveSelection(Vector2 direction)
    {
        var directionFromCamera = rotator.GetCameraOrientedDirection(direction).ToVector3Int();
        var newPosition = _selectedElement.Position + directionFromCamera;
        var newElement = GetElement(newPosition);

        if (newElement)
            ChangeSelectedElement(newElement, ElementFaceNormal);
    }

    public void SelectMiddleElement()
    {
        var pos = GetFacingFaceCentre();

        ChangeSelectedElement(GetElement(pos), GetFacingFaceNormal());
    }

    public void ValidateSelectedElement()
    {
        if (!HasSelectedElement)
            return;

        var centre = Vector3.one * ((Size - 1) / 2f);
        var localDirection = _selectedElement.Position - centre;
        var worldDirection = cubeRoot.TransformDirection(localDirection);
        var cameraDirection = Camera.main.transform.position - cubeRoot.TransformPoint(_selectedElement.Position);
        var dot = Vector3.Dot(worldDirection, cameraDirection);

        if (dot <= 0)
            ChangeSelectedElement(null, Vector3.zero);
        else
        {
            ElementFaceNormal = GetFacingFaceNormal();
            rotator.SetSelectedElementNormal(ElementFaceNormal);
        }
    }

    private Vector3 GetFacingFaceNormal()
    {
        Vector3 cameraDirection = Camera.main.transform.forward;

        float x = Vector3.Dot(cameraDirection, cubeRoot.right);
        float y = Vector3.Dot(cameraDirection, cubeRoot.up);
        float z = Vector3.Dot(cameraDirection, cubeRoot.forward);

        float absX = Mathf.Abs(x);
        float absY = Mathf.Abs(y);
        float absZ = Mathf.Abs(z);

        if (absX > absY && absX > absZ)
            return cubeRoot.right * -Mathf.Sign(x);

        if (absY > absZ)
            return cubeRoot.up * -Mathf.Sign(y);

        return cubeRoot.forward * -Mathf.Sign(z);
    }

    private bool PositionValid(Vector3Int position)
        => position.x >= 0 && position.y >= 0 && position.z >= 0 &&
            position.x < Size && position.x < Size && position.z < Size;

    private Vector3Int GetFacingFaceCentre()
    {
        var normal = GetFacingFaceNormal();
        var middle = (Size - 1) / 2;
        var centre = new Vector3Int(middle, middle, middle);
        var localNormal = cubeRoot.InverseTransformDirection(normal);

        if (Mathf.Abs(localNormal.x) > 0.5f)
            centre.x = localNormal.x > 0 ? Size - 1 : 0;

        else if (Mathf.Abs(localNormal.y) > 0.5f)
            centre.y = localNormal.y > 0 ? Size - 1 : 0;

        else
            centre.z = localNormal.z > 0 ? Size - 1 : 0;

        return centre;
    }

    private CubeElement GetElement(Vector3Int position)
        => _elements.FirstOrDefault(e => e.Position == position);
}
