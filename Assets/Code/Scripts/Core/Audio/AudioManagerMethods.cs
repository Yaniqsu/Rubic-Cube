using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{
    public partial class AudioManager
    {
        public static void PlayAudio(AudioResource clip, string soundTag, Vector3 location, MixerGroups group,
                PlaySettings settings = null)
        {
            settings ??= new PlaySettings();

            Instance.PlaySound(clip, soundTag, null, location, group, settings);
        }

        public static void PlayAudio(AudioResource clip, string soundTag, Transform parent, Vector3 location, MixerGroups group,
                PlaySettings settings = null)
        {
            settings ??= new PlaySettings();

            Instance.PlaySound(clip, soundTag, parent, location, group, settings);
        }

        public static void PlayAudio(AudioResource clip, Vector3 location, MixerGroups group,
            PlaySettings settings = null)
        {
            settings ??= new PlaySettings();

            Instance.PlaySound(clip, string.Empty, null, location, group, settings);
        }

        public static void PlayAudio(
            AudioResource clip,
            Transform parent,
            Vector3 location,
            MixerGroups group,
            PlaySettings settings = null)
        {
            settings ??= new PlaySettings();

            Instance.PlaySound(clip, string.Empty, parent, location, group, settings);
        }

        public static void PlayAudio(
            AudioResource soundData,
            MixerGroups group,
            PlaySettings settings = null)
        {
            settings ??= new PlaySettings();
            settings.global = true;

            Instance.PlaySound(soundData, string.Empty, null, Vector3.zero, group, settings);
        }

        public static void PlayAudio(
            AudioResource soundData,
            string soundTag,
            MixerGroups group,
            PlaySettings settings = null)
        {
            settings ??= new PlaySettings();

            settings.global = true;

            Instance.PlaySound(soundData, soundTag, null, Vector3.zero, group, settings);
        }

        public static void SetPitch(string soundTag, float pitch)
        {
            Instance.SetPitchOnInstance(soundTag, pitch);
        }

        public static void SetVolume(string soundTag, float volume) => Instance.SetVolumeOnInstance(soundTag, volume);
        public static void SetVolumeLevel(string soundTag, float volume) => Instance.SetVolumeLevelOnInstance(soundTag, volume);
        public static void StopAudio(string soundTag, float expireTime = 0) => Instance.StopAudioOnInstance(soundTag, expireTime);
        public static void StopNonPersistentHandlers() => Instance.StopNonPersistentHandlersOnInstance();
        public static void Pause(string soundTag)
            => Instance.PauseOnInstance(Instance._soundProcesses
                .Where(p => string.CompareOrdinal(p.tag, soundTag) == 0)
                .Select(p => p.audioHandler));
        public static void UnPause(string soundTag)
            => Instance.UnPauseOnInstance(Instance._soundProcesses
                .Where(p => string.CompareOrdinal(p.tag, soundTag) == 0)
                .Select(p => p.audioHandler));
        public static void Pause(MixerGroups mixerGroup)
            => Instance.PauseOnInstance(Instance._soundProcesses
                .Select(p => p.audioHandler)
                .Where(p => p.AudioMixerGroup == Instance.GetMixerGroup(mixerGroup)));
        public static void UnPause(MixerGroups mixerGroup)
            => Instance.UnPauseOnInstance(Instance._soundProcesses
                .Select(p => p.audioHandler)
                .Where(p => p.AudioMixerGroup == Instance.GetMixerGroup(mixerGroup)));

        public IEnumerable<AudioResource> GetResources(string soundTag)
            => _soundProcesses.Where(p => string.CompareOrdinal(p.tag, soundTag) == 0)
                                .Select(p => p.resource);
    }
}
