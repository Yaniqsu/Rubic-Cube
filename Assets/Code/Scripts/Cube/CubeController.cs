using System.Linq;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class CubeController : MonoBehaviour
{
    [SerializeField] private CubeSpawner spawner;
    [SerializeField] private CubeRotator rotator;
    [SerializeField] private int defaultSize;
    [SerializeField] private int randomizeTurns;

    private CubeElement[] _elements;
    private bool _preSetting;
    private readonly Stack<CubeRotationData> _rotationHistory = new();

    public int Size { get; private set; }

    public event Action onCubeSolved;
    public event Action onMove;
    public event Action onUndo;

    public void Initialize(int randomizedTurns)
    {
        Size = GameRequest.cubeSize;
        if (Size == 0) Size = defaultSize;
        
        _elements = spawner.SpawnCube(Size).ToArray();

        rotator.Initialize(_elements, Size);
        rotator.onMove += OnMove;
        if (randomizedTurns > 0) rotator.onRotationEnd += OnRotationEnd;
        
        Randomize(randomizedTurns == 0 ? randomizeTurns : randomizedTurns);
    }

    public void Back()
    {
        if (_rotationHistory.Count == 0)
            return;

        var data = _rotationHistory.Pop();

        rotator.ForceRotation(data.elementPos, -data.axis);
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

            rotator.ForceRotation(elementPos, rotation * axis);
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
        onCubeSolved?.Invoke();
        rotator.StartSpinning();
    }

    private void OnMove(CubeRotationData rotationData)
    {
        _rotationHistory.Push(rotationData);
        
        onMove?.Invoke();
    }
}
