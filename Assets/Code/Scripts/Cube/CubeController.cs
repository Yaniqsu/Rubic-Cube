using System.Linq;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class CubeController : MonoBehaviour
{
    [SerializeField] private CubeSpawner spawner;
    [SerializeField] private CubeRotator rotator;
    [SerializeField] private int defaultSize;
    [SerializeField] private int randomizeTurns;

    private CubeElement[] _elements;

    public int Size { get; private set; }

    public event Action onCubeSolved;
    public event Action onMove;

    public void Initialize(int randomizedTurns)
    {
        Size = GameRequest.cubeSize;
        if (Size == 0) Size = defaultSize;
        
        _elements = spawner.SpawnCube(Size).ToArray();

        rotator.Initialize(_elements, Size);
        rotator.onMove += () => onMove?.Invoke();
        if (randomizedTurns > 0) rotator.onRotationEnd += OnRotationEnd;
        
        Randomize(randomizedTurns == 0 ? randomizeTurns : randomizedTurns);
    }

    private void OnRotationEnd()
    {
        if (IsCubeSolved())
            OnCubeSolved();
    }

    private void Randomize(int turns)
    {
        for (var i = 0; i < turns; i++)
        {
            var elementPos = new Vector3Int(
                Random.Range(0, Size),
                Random.Range(0, Size),
                Random.Range(0, Size));

            var rotation = Random.Range(1, 4) * (Random.Range(0, 2) == 0 ? 1 : -1) * 90;
            var axis = Random.Range(0, 3) switch
            {
                0 => Vector3.right,
                1 => Vector3.up,
                2 => Vector3.forward,
                _ => Vector3.zero
            };

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
        Debug.Log("Solved");
        onCubeSolved?.Invoke();
    }
}
