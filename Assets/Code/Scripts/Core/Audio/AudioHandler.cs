using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        private float _volume = 1;
        private float _volumeLevel = 1;
        private float _maxDistance;
        private float _time;
        private bool _followObject;
        private Transform _followTransform;

        public bool Paused { get; private set; }
        public bool Persistent { get; set; }

        public AudioSource AudioSource
        {
            get
            {
                _audioSource ??= GetComponent<AudioSource>();

                return _audioSource;
            }
        }

        public void Initialize()
        {
            _maxDistance = _audioSource.maxDistance;;
        }
        
        public bool CanRead => _audioSource != null;

        public float VolumeLevel
        {
            get => _volumeLevel;
            set
            {
                
                _volumeLevel = value;
                /*if(AudioSource.resource)
                    Debug.Log($"{AudioSource.resource.name}: {_volumeLevel}");*/
                AudioSource.volume = _volume * _volumeLevel;
            }
        }

        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                AudioSource.volume = _volume * _volumeLevel;
            }
        }

        public float Pitch
        {
            get => AudioSource.pitch;
            set => AudioSource.pitch = value;
        }

        public AudioResource Clip
        {
            get => AudioSource.resource;
            set => AudioSource.resource = value;
        }

        public float Time
        {
            get => AudioSource.time;
            set 
            { 
                AudioSource.time = value;
                _time = value;
            }
        }

        public bool Global
        {
            set => AudioSource.spatialBlend = value ? 0 : 1;
        }

        public AudioMixerGroup AudioMixerGroup
        {
            get => AudioSource.outputAudioMixerGroup;
            set => AudioSource.outputAudioMixerGroup = value;
        }
        
        public bool IsPlaying => AudioSource.isPlaying;

        public bool Looping
        {
            get => AudioSource.loop;
            set => AudioSource.loop = value;
        }

        public Transform Follow
        {
            get => _followTransform;
            set
            {
                _followTransform = value;
                _followObject = value;
            }
        }

        private void LateUpdate()
        {
            if(_followObject)
            {
                if(!_followTransform)
                {
                    _followObject = false;
                    return;
                }
                
                transform.position = _followTransform.position;
            }
        }

        public void Play()
        {
            UnPause();
            AudioSource.Play();
        }

        public void Stop() => _audioSource?.Stop();
        public void Pause() 
        {
            Paused = true;
            AudioSource?.Pause();
        }
        public void UnPause() 
        {
            Paused = false;
            AudioSource?.UnPause();
        }
    }
}
