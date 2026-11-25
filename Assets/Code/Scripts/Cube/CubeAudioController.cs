using SoundSystem;
using UnityEngine;
using UnityEngine.Audio;

public class CubeAudioController : MonoBehaviour 
{
    private const string SEGMENT_MOVE_SOUND = "Segment Move Sound";
    
    [SerializeField] private ACubeRotator cubeRotator;
    [SerializeField] private CubeController cubeController;
    [Space]
    [SerializeField] private AudioResource moveSegmentSound;
    [SerializeField] private AudioResource snapSound;
    [SerializeField] private AudioResource changeSelectionSound;

    private bool _dragging;
    private bool _playingMoveSound;

    private void Start()
    {
        cubeController.OnMoveSelection += () =>
        {
            AudioManager.PlayAudio(changeSelectionSound, MixerGroups.SFX);
        };
        
        cubeRotator.OnStartRotation += () =>
        {
            _dragging = true;
            _playingMoveSound = true;
            AudioManager.PlayAudio(moveSegmentSound, SEGMENT_MOVE_SOUND, MixerGroups.SFX);
        };

        cubeRotator.OnEndRotation += () =>
        {
            _dragging = false;
            _playingMoveSound = false;
            AudioManager.StopAudio(SEGMENT_MOVE_SOUND, 0.1f);
        };

        cubeRotator.OnSnap += () =>
        {
            AudioManager.PlayAudio(snapSound, MixerGroups.SFX);
        };
    }

    public void PauseMoveSound()
    {
        if(_dragging && _playingMoveSound)
        {
            AudioManager.StopAudio(SEGMENT_MOVE_SOUND, 0.1f);
            _playingMoveSound = false;
        }
    }

    public void ResumeMoveSound()
    {
        if (_dragging && !_playingMoveSound)
        {
            AudioManager.PlayAudio(moveSegmentSound, SEGMENT_MOVE_SOUND, MixerGroups.SFX);
            _playingMoveSound = true;
        }
    }
}
