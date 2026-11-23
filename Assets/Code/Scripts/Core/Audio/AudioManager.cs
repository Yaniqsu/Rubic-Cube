using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Audio;
using YNQ;
using Debug = UnityEngine.Debug;

namespace SoundSystem
{
    public partial class AudioManager : MonoBehaviour
    {
        [System.Serializable]
        private struct MixerGroup
        {
            public MixerGroups groupEnum;
            public AudioMixerGroup mixerGroup;
        }

        private static AudioManager _instance;

        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instance = Resources.Load<AudioManager>("AudioManager");
                    _instance = Instantiate(instance);
                    DontDestroyOnLoad(_instance);
                }
                
                return _instance;
            }
        }

        [SerializeField] private AudioHandler audioHandler;
        [SerializeField] private MixerGroup[] mixerGroups;

        private ObjectPool<AudioHandler> _audioPool;
        private List<SoundProcess> _soundProcesses;
        private List<SoundProcess> _processesToStopCoroutine = new List<SoundProcess>();

        private void Awake()
        {
            if(_instance != null)
                Destroy(gameObject);
            
            _audioPool = new ObjectPool<AudioHandler>(
                audioHandler, 
                0, 999, transform, 
                new PoolSettings() { 
                    hideFlags = HideFlags.None
                }, 
                handler => handler.Initialize());

            _soundProcesses = new List<SoundProcess>();
        }

        private void LateUpdate()
        {
            if (_processesToStopCoroutine.Count == 0)
                return;
            
            foreach(var process in _processesToStopCoroutine)
            {
                if(process.c_currentCoroutine != null)
                    StopCoroutine(process.c_currentCoroutine);
                
                process.audioHandler?.Stop();
                _soundProcesses.Remove(process);
            }
            _processesToStopCoroutine.Clear();
        }

        private void PlaySound(AudioResource clip, string soundTag, Transform parent, Vector3 location, MixerGroups group, PlaySettings settings)
        {
            if (clip == null)
            {
                Debug.LogWarning("Audio clip is null");
                return;
            }
            
            if(!string.IsNullOrEmpty(soundTag))
            {
                var proceess = _soundProcesses.FirstOrDefault(p => p.tag.Equals(soundTag));

                if (proceess != null)
                {
                    if (settings.priority > proceess.priority && settings.stopLowerPriority)
                        StopProcess(proceess);
                    else if(settings.priority <= proceess.priority && settings.mustBeHighestPriority)
                        return;
                }
            }

            var handler = _audioPool.GetFreeObject();

            handler.name = clip.name;
            handler.Follow = parent;
            handler.transform.position = location;
            handler.AudioMixerGroup = GetMixerGroup(group);
            handler.Global = settings.global;
            handler.Looping = settings.looping;
            handler.Persistent = settings.persistent;
            handler.gameObject.SetActive(true);
            
            var coroutine = StartCoroutine(EmergeSource(handler, settings.emergencyTime, settings.volumeLevel));
            var soundProcess = new SoundProcess(soundTag, settings.volumeLevel, handler, clip, coroutine, settings.priority);
            _soundProcesses.Add(soundProcess);

            if (parent != null)
            {
                var obj = new GameObject($"[{clip.name}] Persistent Handler");
                obj.transform.SetParent(parent);
                var controller = obj.AddComponent<AudioInstanceController>();
                controller.SoundProcess = soundProcess;
                controller.onStopSoundProcess += StopProcess;
            }

            StartCoroutine(HandleAudioSource(soundProcess));
        }
        // ReSharper disable Unity.PerformanceAnalysis
        private void StopAudioOnInstance(string soundTag, float expireTime)
        {
            foreach (var process in _soundProcesses.Where(process => process.tag == soundTag))
            {
                process.tag = string.Empty;
                
                if(expireTime == 0)
                    process.audioHandler?.Stop();
                else
                {
                    process.stopping = true;
                    ChangeCoroutineOnSoundProcesses(soundTag, StartCoroutine(StopSoundCoroutine(process.audioHandler, expireTime)));
                }
            }
        }

        private void SetPitchOnInstance(string soundTag, float pitch)
        {
            var processes = _soundProcesses.Where(process => process.tag == soundTag);

            foreach (var process in processes)
            {
                process.audioHandler.Pitch = pitch;
            }
        }

        private void SetVolumeOnInstance(string soundTag, float volume)
        {
            var processes = _soundProcesses.Where(process => process.tag == soundTag);

            foreach (var process in processes)
            {
                process.audioHandler.Volume = volume;
            }
        }
        
        private void SetVolumeLevelOnInstance(string soundTag, float volume)
        {
            var processes = _soundProcesses.Where(process => process.tag == soundTag).ToArray();
            foreach (var process in processes)
            {
                process.audioHandler.VolumeLevel = volume;
            }
        }

        private void PauseOnInstance(IEnumerable<AudioHandler> handlers)
        {
            foreach (var handler in handlers)
            {
                handler.Pause(); 
            }
        }

        private void UnPauseOnInstance(IEnumerable<AudioHandler> handlers)
        {
            foreach (var handler in handlers)
            {
                handler.UnPause();
            }
        }

        public bool IsPlaying(string soundTag)
        {
            return _soundProcesses.Any(process => process.tag == soundTag);
        }

        public float GetTime(string soundTag)
        {
            var process = _soundProcesses.Find(process => process.tag == soundTag);
            return process == null ? 0 : process.audioHandler.Time;
        }

        private void ChangeCoroutineOnSoundProcesses(string soundTag, Coroutine coroutine)
        {
            var processes = _soundProcesses.Where(s => s.tag == soundTag);
            
            foreach (var process in processes)
            {
                if(process.c_currentCoroutine != null)
                    StopCoroutine(process.c_currentCoroutine);
                
                process.c_currentCoroutine = coroutine;
            }
        }

        private AudioMixerGroup GetMixerGroup(MixerGroups group)
        {
            return mixerGroups.FirstOrDefault(g => g.groupEnum == group).mixerGroup;
        }
        private static IEnumerator StopSoundCoroutine(AudioHandler handler, float expireTime)
        {
            yield return null;
            
            if (handler == null)
                yield break;
            
            float currentTime = 0;
            while(currentTime < expireTime)
            {
                handler.VolumeLevel = Mathf.Lerp(1, 0, currentTime / expireTime);
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }

            handler.VolumeLevel = 0;
            handler.Stop();
        }
        private static IEnumerator EmergeSource(AudioHandler handler, float emergencyTime, float volumeLevel)
        {
            yield return null;
            if (handler == null)
            {
                yield break;
            }
            
            float currentTime = 0;
            while (currentTime < emergencyTime)
            {
                handler.VolumeLevel = Mathf.Lerp(0, volumeLevel, currentTime / emergencyTime);
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }

            handler.VolumeLevel = volumeLevel;
        }
        private IEnumerator HandleAudioSource(SoundProcess soundProcess)
        {
            yield return null;

            soundProcess.audioHandler.Clip = soundProcess.resource;
            soundProcess.audioHandler.Play();

            if (soundProcess.audioHandler.Looping)
                yield break;

            yield return null;
            yield return new WaitWhile(() => soundProcess.audioHandler.IsPlaying);
            
            StopProcess(soundProcess);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void StopProcess(SoundProcess proccess)
        {
            if (!_soundProcesses.Contains(proccess))
                return;

            if (proccess.audioHandler)
            {
                proccess.audioHandler.Stop();
                proccess.audioHandler.gameObject.SetActive(false);
            }
           
            _processesToStopCoroutine.Add(proccess);
        }
        private AudioMixerGroup GetAudioMixerGroup(MixerGroups mixerGroup) 
            => mixerGroups
            .First(group => group.groupEnum == mixerGroup)
            .mixerGroup;

        private void StopNonPersistentHandlersOnInstance()
        {
            foreach(var process in _soundProcesses)
            {
                if(!process.audioHandler.Persistent)
                    StopProcess(process);
            }
        }
    }
}