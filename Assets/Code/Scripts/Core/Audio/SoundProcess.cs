using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{
    [System.Serializable]
    public class SoundProcess
    {
        public string tag;
        public float volumeLevel;
        public bool stopping;
        public int priority;
        public AudioHandler audioHandler;
        public AudioResource resource;
        public Coroutine c_handleSound;
        public Coroutine c_currentCoroutine;

        public SoundProcess(string tag, float volumeLevel, AudioHandler audioHandler, AudioResource resource, Coroutine currentCoroutine, int priority)
        {
            this.tag = tag;
            this.volumeLevel = volumeLevel;
            this.audioHandler = audioHandler;
            this.resource = resource;
            c_currentCoroutine = currentCoroutine;
            this.priority = priority;
        }
    }
}