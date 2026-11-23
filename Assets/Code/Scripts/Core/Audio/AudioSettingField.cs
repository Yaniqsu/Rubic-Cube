using UnityEngine;
using UnityEngine.Audio;
using YNQ.Settings.SettingField;

public class AudioSettingField : AScriptableSettingField<int>
{
    [field: SerializeField] private AudioMixerGroup MixerGroup { get; set; }
    
    public override string GetValueDisplay() => TempValue.ToString();

    protected override int ValidateValue(int value) => Mathf.Clamp(value, 0, 100);
}
