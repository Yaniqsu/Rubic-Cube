using SoundSystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameWonManager : MonoBehaviour
{
    [SerializeField] private AudioResource gameWonSound;
    [SerializeField] private GameWonUIManager uiManager;
    
    
    public void Show()
    {
        AudioManager.PlayAudio(gameWonSound, MixerGroups.UI);
        
        uiManager.Show();
    }

    public void DisplayStats(GameStats stats)
    {
        uiManager.DisplayStats(stats);
    }
    
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
