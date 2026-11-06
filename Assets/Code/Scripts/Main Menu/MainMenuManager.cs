using UnityEngine;
using YNQ.Settings;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private SetupManager setupManager;

    private IMenuComponent _currentMenuComponent;
    
    public void ShowSettingsMenu()
    {
        RegisterMenuComponent(settingsManager);
    }

    public void ShowSetup()
    {
        RegisterMenuComponent(setupManager);
    }
    
    public void Back()
    {
        if(_currentMenuComponent != null)
            _currentMenuComponent.Back();

        _currentMenuComponent = null;
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    private void RegisterMenuComponent(IMenuComponent menuComponent)
    {
        if (_currentMenuComponent != null)
            _currentMenuComponent.Back();

        _currentMenuComponent = menuComponent;
        _currentMenuComponent.Show();
    }
}
