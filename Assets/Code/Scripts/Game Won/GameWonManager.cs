using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWonManager : MonoBehaviour
{
    [SerializeField] private GameWonUIManager uiManager;
    
    public void Show()
    {
        uiManager.Show();
    }

    public void DisplayStats(GameStats stats)
    {

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
