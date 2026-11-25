using SoundSystem;
using UnityEngine;
using UnityEngine.Audio;

public class ObjectRotationAudioController : MonoBehaviour
{
    [SerializeField] private ObjectRotationController rotationController;
    [Space]
    [SerializeField] private AudioResource snapAndRotateSound;
    [SerializeField] private AudioResource snapAndTiltSound;

    private void Start()
    {
        rotationController.OnSnapAndRotate += () =>
        {
            AudioManager.PlayAudio(snapAndRotateSound, MixerGroups.SFX);
        };

        rotationController.OnSnapAndTilt += () =>
        {
            AudioManager.PlayAudio(snapAndTiltSound, MixerGroups.SFX);
        };
    }
}
