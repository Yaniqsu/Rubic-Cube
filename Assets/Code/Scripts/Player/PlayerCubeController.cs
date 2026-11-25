using SoundSystem;
using UnityEngine;
using UnityEngine.Audio;
using YNQ.Sequencer;
using YNQ.Settings.SettingField;

public class RotateCubeAction : ISequenceAction
{
    private readonly ObjectRotationController _rotationController;
    private readonly CameraCubeRotator _cubeRotator;
    private readonly Vector2Int _direction;
    private readonly float _time;
    private float _elapsed;

    public RotateCubeAction(
        ObjectRotationController rotationController, CameraCubeRotator cubeRotator, 
        Vector2Int direction, float time)
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
    [SerializeField] private CubeAudioController cubeAudioController;
    [SerializeField] private CameraCubeRotator cubeRotator;
    [SerializeField] private ObjectRotationController rotationController;
    [SerializeField] private MouseRaycastController mouseRaycastController;
    [SerializeField] private IntSettingField settingFreeRotation;
    [SerializeField] private IntSettingField settingMouseRotation;
    [SerializeField] private IntSettingField settingMouseDrag;
    [SerializeField] private FloatSettingField settingActionTime;
    [Space]
    [SerializeField] private float errorMargin;

    private float _totalDragLength;
    private bool _dragging;
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
                settingActionTime.Value));
        }
        else
        {
            rotationController.Rotate(direction, Time.deltaTime * settingFreeRotation.Value);
        }
    }

    public void MoveSegment(Vector2 direction)
    {
        if(cubeController.HasSelectedElement)
        {
            var position = cubeController.SelectedElementPosition;
            
            _sequencer.QueueAction(new SetRotationAction(cubeRotator, direction, settingActionTime.Value), () =>
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
        _sequencer.QueueAction(new SnapAndTiltAction(rotationController, cubeRotator, (int)direction, settingActionTime.Value));
    }

    public void StartRotation()
    {
        if (cubeController.HasSelectedElement)
        {
            _totalDragLength = 0;
            
            cubeRotator.StartRotationDrag();
        }
    }

    public void RotateCube(Vector2 mouseDelta)
    {
        rotationController.Rotate(mouseDelta, Time.deltaTime * settingMouseRotation.Value);
    }

    public void ExecuteRotation(Vector2 mouseDelta)
    {
        var magnitude = mouseDelta.magnitude;
        
        _totalDragLength += magnitude;
        if (_totalDragLength < errorMargin)
        {
            return;
        }

        HandleDragSegmentAudio(mouseDelta.magnitude);

        cubeRotator.RotateByCameraDirection(mouseDelta, Time.deltaTime * settingMouseDrag.Value);
    }

    public void EndRotation() => cubeRotator.EndRotationDrag();

    private void HandleDragSegmentAudio(float mouseDeltaMagnitude)
    {
        if (mouseDeltaMagnitude > 1.5f)
        {
            if (!_dragging)
            {
                _dragging = true;
                cubeAudioController.ResumeMoveSound();
            }
        }
        else if(mouseDeltaMagnitude == 0)
        {
            if (_dragging)
            {
                _dragging = false;
                cubeAudioController.PauseMoveSound();
            }
        }
    }
}
