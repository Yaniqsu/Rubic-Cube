using System;
using System.Collections.Generic;
using NaughtyAttributes;

using UnityEngine;
using UnityEngine.Audio;
using Utilities;
using YNQ.Settings.SettingField;

[Serializable]
public struct MixerGroupSetting
{
    [ReadOnly] public IntSettingField audioSetting;
    [ReadOnly] public AudioMixerGroup mixerGroup;
    public Action<int> updateMixerGroup; 
}

public class AudioSettings : ASettingsRuntimeManager
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField, AlwaysExpanded(true)] private MixerGroupSetting[] settings;

    private readonly Dictionary<IntSettingField, Action<int>> handlers = new();

    protected override void Connect()
    {
        foreach(var s in settings)
        {
            void handler(int value) =>
                ChangeMixerSettings(s.mixerGroup.name, (float)value / s.audioSetting.MaxValue);

            s.audioSetting.OnValueChanged += handler;
            handlers.TryAdd(s.audioSetting, handler);
        }
    }

    protected override void Disconnect()
    {
        foreach (var s in settings)
        {
            if (handlers.TryGetValue(s.audioSetting, out var handler))
            {
                s.audioSetting.OnValueChanged -= handler;
            }
        }

        handlers.Clear();
    }

    private void ChangeMixerSettings(string parameter, float volume)
    {
        volume = Mathf.Max(0.0001f, volume);

        mixer.SetFloat(parameter, Mathf.Log10(volume) * 20);
    }
}
