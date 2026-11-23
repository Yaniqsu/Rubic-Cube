using NaughtyAttributes;
using UnityEngine;
using Utilities;

namespace SoundSystem
{
    [System.Serializable]
    public class AudioData
    {
        public AudioClip audioClip;
        public float volume = 1;
        public float pitch = 1;
    }

    public enum PlayType
    {
        One = 0,
        AllClips = 1,
        SpecifiedAmount = 2,
    }
    
    [CreateAssetMenu(fileName = "NewSoundData", menuName = "Sound System/Sound Data")]
    public class SoundData : ScriptableObject
    {
        public float Volume => Random.Range(volume.x, volume.y);
        public float Pitch => Random.Range(pitch.x, pitch.y);
        public int ClipsAmount => Mathf.FloorToInt(Random.Range(clipsAmount.x, clipsAmount.y));
        public float ClipsInterval => Random.Range(clipsInterval.x, clipsInterval.y);
        public AudioData RandomClip => clips[Random.Range(0, clips.Length)];
        public AudioData[] ClipsShuffled => clips;
        
        [MinMaxSlider(0, 10)] 
        [SerializeField] private Vector2 volume = new Vector2(1, 1);
        
        [MinMaxSlider(0, 10)] 
        [SerializeField] private Vector2 pitch = new Vector2(1, 1);
        
        public bool looping;
        
        public PlayType playType;

        [MinMaxSlider(0, 10)] 
        [SerializeField] private Vector2Int clipsAmount = new (1,1);
        
        [MinMaxSlider(0, 50)] 
        public Vector2 clipsInterval;
        
        [AlwaysExpanded(true)] 
        public AudioData[] clips;
    }
}