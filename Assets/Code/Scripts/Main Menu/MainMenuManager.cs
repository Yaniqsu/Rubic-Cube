using UnityEngine;
using YNQ.Settings;

public class MainMenuManager : AMenuManager
{
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private SetupManager setupManager;

    public void ShowSettingsMenu()
    {
        RegisterMenuComponent(settingsManager);
    }

    public void ShowSetup()
    {
        RegisterMenuComponent(setupManager);
    }

    protected override void OnBack() { }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
