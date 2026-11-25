using SoundSystem;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenuMusicManager : MonoBehaviour
{
    [SerializeField] private AudioResource mainMenuMusic;

    private void Start()
    {
        if (!AudioManager.Instance.IsPlaying(GlobalAudioTags.LevelMusic))
        {
            AudioManager.PlayAudio(mainMenuMusic, GlobalAudioTags.LevelMusic, MixerGroups.Music, new PlaySettings()
            {
                emergencyTime = 3f,
                persistent = true,
                looping = true
            });
        }
    }
}
