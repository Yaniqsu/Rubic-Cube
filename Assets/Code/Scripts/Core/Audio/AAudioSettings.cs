using UnityEngine;
using UnityEngine.Audio;

public abstract class AAudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    
    protected void ChangeMixerSettings(string parameter, float volume)
    {
        volume = Mathf.Max(0.0001f, volume);
        
        _audioMixer.SetFloat(parameter, Mathf.Log10(volume) * 20);
    }
    
    public abstract void SaveSettings();
    public abstract void LoadSettings();

    protected static float GetVolume(string key)
    {
        if(PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetFloat(key);
        
        PlayerPrefs.SetFloat(key, 1);
        return 1;
    }
}
