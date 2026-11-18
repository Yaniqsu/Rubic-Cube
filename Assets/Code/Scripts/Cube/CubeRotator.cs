using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CoroutineSequencer
{
    private struct RoutineData
    {
        public IEnumerator enumerator;
        public Action onEnd;
    }
    
    private readonly Queue<RoutineData> _queue;
    private readonly MonoBehaviour _mono;
    private Coroutine _sequenceRoutine;

    public CoroutineSequencer(MonoBehaviour mono)
    {
        _mono = mono;
        _queue = new Queue<RoutineData>();
    }

    public void AddCoroutine(IEnumerator enumerator, Action onEnd = null)
    {
        _queue.Enqueue(new RoutineData { enumerator = enumerator, onEnd = onEnd });

        StartSequence();
    }

    private void StartSequence()
    {
        if(_sequenceRoutine == null) 
            _sequenceRoutine = _mono.StartCoroutine(SequenceRoutine());
    }

    private IEnumerator SequenceRoutine()
    {
        while(_queue.Count > 0)
        {
            var data = _queue.Dequeue();
            
            yield return data.enumerator;

            data.onEnd?.Invoke();
        }

        _sequenceRoutine = null;
    }
}

public class CubeRotator : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float errorMargin;
    [SerializeField] private Transform cubeRoot;
    [SerializeField] private LayerMask cubeLayer;
    [SerializeField] private Transform pivot;

    private CubeController _cubeController;
    private CubeElement[] _elements;
    private Camera _main;
    private CoroutineSequencer _moveSegmentSequencer;

    private int _size;
    private float _dragLength;
    private Vector3 _setRotationAxis;
    private Vector3Int _elementPosition;
    private Vector3 _elementFaceNormal;
    private Vector3 _totalRotation;

    private CubeElement[] _elementsToRotate = Array.Empty<CubeElement>();

    public event Action onRotationEnd;
    public event Action<CubeRotationData> onMove;


    public Quaternion CameraRelativeRotation => Quaternion.Inverse(_main.transform.rotation) * cubeRoot.localRotation;
    public Vector3 CameraRelativeRotationEuler => CameraRelativeRotation.eulerAngles;
    public bool Rotating { get; private set; }

    private void Awake()
    {
        _main = Camera.main;
        _moveSegmentSequencer = new CoroutineSequencer(this);
    }

    public void Initialize(CubeController cubeController, CubeElement[] elements, int size)
    {
        _cubeController = cubeController;
        _elements = elements;
        _size = size;
    }

    #region INTERFACE

    public void StartRotation()
    {
        if (_cubeController.CurrentSelectedElement)
        {
            InitializeRotation(_cubeController.CurrentSelectedElement, _cubeController.ElementFaceNormal);
        }
    }

    public void ExecuteRotation(Vector2 mouseDelta)
    {
        RotateFromDirection(mouseDelta, Time.deltaTime * speed);
    }

    public void EndRotation()
    {
        CleanAfterRotation();

        if (_totalRotation.magnitude >= 90)
            onMove?.Invoke(new CubeRotationData() { elementPos = _elementPosition, axis = _totalRotation });

        _totalRotation = Vector3.zero;
        onRotationEnd?.Invoke();
    }

    public void ForceRotationFromCamera(Vector2 direction, float time)
    {
        if (!_cubeController.CurrentSelectedElement)
            return;
        
        InitializeRotation(_cubeController.CurrentSelectedElement, _cubeController.ElementFaceNormal);
        
        RotateFromDirectionInTime(direction, 90f, time, EndRotation);
    }

    public void ForceRotation(Vector3Int elementPos, Vector3 axis)
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

    public void StartSpinning()
    {
        StartCoroutine(HandleSpinning());
    }

    #endregion

    private void RotateFromDirection(Vector2 direction, float speed, bool ignoreMargin = false)
    {
        if (!Rotating || direction.magnitude < 0.1f)
            return;

        _dragLength += direction.magnitude;

        if (!ignoreMargin && _dragLength < errorMargin)
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

    private void RotateFromDirectionInTime(Vector2 direction, float speed, float time, Action onEnd = null)
    {
        _dragLength += direction.magnitude;

        var localAxis = GetRotationAxis(direction);

        ChooseRotationAxis(localAxis);

        _totalRotation += speed * localAxis;
        _moveSegmentSequencer.AddCoroutine(RotationInTimeRoutine(speed * localAxis, time), onEnd);
    }

    private IEnumerator RotationInTimeRoutine(Vector3 rotationEuler, float time)
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

    private Vector3 GetRotationAxis(Vector2 direction)
    {
        var dragDirection = GetRotationAxisFromDirection(direction);
        var localDrag = cubeRoot.InverseTransformDirection(dragDirection).normalized;
        var rotationAxis = Vector3.Cross(_elementFaceNormal, localDrag).normalized;
        var localAxis = transform.InverseTransformDirection(rotationAxis);
        localAxis = localAxis.GetClosestAxis();

        return localAxis;
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
        _dragLength = 0;
        _setRotationAxis = Vector3.zero;
        Rotating = false;
    }

    private void InitializeRotation(CubeElement element, Vector3 normal)
    {
        _elementPosition = element.Position;
        _elementFaceNormal = cubeRoot.InverseTransformDirection(normal).normalized;
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

    private IEnumerator HandleSpinning()
    {
        var speed= 0f;
        var axis = cubeRoot.InverseTransformDirection(Vector3.down);
        while(true)
        {
            speed = Mathf.MoveTowards(speed, 20, 5 * Time.deltaTime);
            cubeRoot.Rotate(speed * Time.deltaTime * axis);
            yield return null;
        }
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

    private Vector3 GetRotationAxisFromDirection(Vector2 direction)
        => _main.transform.right * direction.x + _main.transform.up * direction.y;

    #endregion
}
