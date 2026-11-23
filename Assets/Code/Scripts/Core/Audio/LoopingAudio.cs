using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio/Looping Audio", fileName = "Looping Audio")]
public class LoopingAudio : AudioResource
{
    [SerializeField] private AudioClip Clip;
    [SerializeField] private int Volume = 0;
    [SerializeField] private bool RandomizeVolume;
    [SerializeField] private Vector2Int VolumeRandomized;
    [SerializeField] private int Pitch = 0;
    [SerializeField] private bool RandomizePitch;
    [SerializeField] private Vector2Int PitchRandomized;
}
