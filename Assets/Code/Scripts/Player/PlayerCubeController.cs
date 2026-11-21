using System;
using UnityEngine;
using YNQ.Sequencer;

public class RotateCubeAction : ISequenceAction
{
    private readonly ObjectRotationController _rotationController;
    private readonly CameraCubeRotator _cubeRotator;
    private readonly Vector2Int _direction;
    private readonly float _time;
    private float _elapsed;

    public RotateCubeAction(ObjectRotationController rotationController, CameraCubeRotator cubeRotator, Vector2Int direction, float time)
    {
        _rotationController = rotationController;
        _cubeRotator = cubeRotator;
        _direction = direction;
        _time = time;
        _elapsed = 0;
    }

    public void Start()
    {
        _rotationController.SnapAndRotate(_cubeRotator.CameraRelativeRotationEuler, _direction, _time);
    }

    public void Update(float elapsed)
    {
        _elapsed = elapsed;
    }

    public bool Completed => _elapsed >= _time;
}

public class SetRotationAction : ISequenceAction
{
    private readonly CameraCubeRotator _rotator;
    private readonly Vector2 _direction;
    private readonly float _time;
    private float _elapsed;

    public SetRotationAction(CameraCubeRotator rotator, Vector2 direction, float time)
    {
        _rotator = rotator;
        _direction = direction;
        _time = time + 0.05f;
        _elapsed = 0;
    }

    public void Start()
    {
        _rotator.SetRotationByCameraDirection(_direction, _time);
    }

    public void Update(float elapsed)
    {
        _elapsed = elapsed;
    }

    public bool Completed => _elapsed >= _time;
}

public class SnapAndTiltAction : ISequenceAction
{
    private readonly ObjectRotationController _rotationController;
    private readonly CameraCubeRotator _cubeRotator;
    private readonly int _direction;
    private readonly float _time;
    private float _elapsed;

    public SnapAndTiltAction(ObjectRotationController rotationController, CameraCubeRotator cubeRotator, int direction, float time)
    {
        _rotationController = rotationController;
        _cubeRotator = cubeRotator;
        _direction = direction;
        _time = time;
        _elapsed = 0;
    }

    public void Start()
    {
        _rotationController.SnapAndTilt(_cubeRotator.CameraRelativeRotationEuler, _direction, _time);
    }

    public void Update(float elapsed)
    {
        _elapsed = elapsed;
    }

    public bool Completed => _elapsed >= _time;
}

public class PlayerCubeController : MonoBehaviour
{
    [SerializeField] private CubeController cubeController;
    [SerializeField] private CameraCubeRotator cubeRotator;
    [SerializeField] private ObjectRotationController rotationController;
    [SerializeField] private MouseRaycastController mouseRaycastController;
    [Space]
    [SerializeField] private float freeRotationSpeed;
    [SerializeField] private float mouseRotationSpeed;
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float snapAndRotateTime;
    [SerializeField] private float errorMargin;

    private float _totalDragLength;
    private Sequencer _sequencer;

    private void Start()
    {
        _sequencer = new Sequencer(this);
        
        mouseRaycastController.OnCheck.AddListener(hit =>
        {
            var hitInfo = mouseRaycastController.LastHitInfo;
            if (hit && hitInfo.collider.gameObject.TryGetComponent(out CubeElement element))
                cubeController.ChangeSelectedElement(element, hitInfo.normal);
            else if(!hit)
                cubeController.ChangeSelectedElement(null, Vector3.zero);
        });
    }
    
    public void MoveCube(Vector2Int direction, bool snapRotation)
    {
        if(snapRotation)
        {
            _sequencer.QueueAction(new RotateCubeAction(
                rotationController, 
                cubeRotator, 
                direction, 
                snapAndRotateTime));
        }
        else
        {
            rotationController.Rotate(direction, Time.deltaTime * freeRotationSpeed);
        }
    }

    public void MoveSegment(Vector2 direction)
    {
        if(cubeController.HasSelectedElement)
        {
            var position = cubeController.SelectedElementPosition;
            
            _sequencer.QueueAction(new SetRotationAction(cubeRotator, direction, 0.3f), () =>
            {
                if (position.HasValue)
                {
                    cubeController.SetSelectedElement(position.Value);
                }
            });
        }
    }

    public void MoveSelectedElement(Vector2 direction)
    {
        cubeController.ValidateSelectedElement();

        if(cubeController.HasSelectedElement)
        {
            cubeController.MoveSelection(direction);
        }
        else
        {
            cubeController.SelectMiddleElement();
        }
    }

    public void Tilt(float direction, bool snap)
    {
        _sequencer.QueueAction(new SnapAndTiltAction(rotationController, cubeRotator, (int)direction, 0.3f));
    }

    public void StartRotation()
    {
        if (cubeController.HasSelectedElement)
        {
            _totalDragLength = 0;
            
            cubeRotator.StartRotationDrag();
        }
    }

    public void ExecuteRotation(Vector2 mouseDelta)
    {
        _totalDragLength += mouseDelta.magnitude;
        if (_totalDragLength < errorMargin)
            return;
        
        cubeRotator.RotateByCameraDirection(mouseDelta, Time.deltaTime * mouseRotationSpeed);
    }

    public void EndRotation() => cubeRotator.EndRotationDrag();
}
