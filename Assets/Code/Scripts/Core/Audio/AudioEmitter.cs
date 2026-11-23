using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{
    public class AudioEmitter : MonoBehaviour
    {
        [SerializeField] private AudioResource _audioResource;
        [SerializeField] private MixerGroups _mixerGroup;
        [SerializeField] private string _soundTag;
        [SerializeField] private bool _generateUniqueSoundTag;
        [SerializeField] private bool _global;
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private bool _looping;
        [SerializeField] private int _priority;
        [SerializeField] private float _fadeInTime;

        public void Play()
        {
            if(_global)
                PlayAudioGlobally();
            else
            {
                PlayAudioLocally();
            }
        }

        private void PlayAudioLocally()
        {
            AudioManager.PlayAudio(_audioResource, _soundTag, transform, transform.position, _mixerGroup, CreatePlaySettings());
        }

        private void PlayAudioGlobally()
        {
            AudioManager.PlayAudio(_audioResource, _soundTag, _mixerGroup, CreatePlaySettings());
        }

        private PlaySettings CreatePlaySettings()
            => new()
            {
                emergencyTime = _fadeInTime,
                looping = _looping,
                priority = _priority
            };
    }
}
