using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using YNQ.Settings;

public class PauseMenuManager : AMenuManager
{
    [SerializeField] private PauseMenuUI menuUI;
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private DialogBox dialogBox;
    [SerializeField] private DialogContent goToMainMenuContent;
    [SerializeField] private DialogContent quitContent;

    private bool _paused;

    public static event Action onPause;
    public static event Action onUnpause;

    private void Start()
    {
        settingsManager.onShowAnimationEnd += () => menuUI.ToggleCanvas(false);
        settingsManager.onBackStarted.AddListener(() => menuUI.ToggleCanvas(true));
    }

    public void TogglePauseMenu()
    {
        if (_paused)
            Back();
        else
            ShowPauseMenu();
    }
    
    public void ShowSettings()
    {
        RegisterMenuComponent(settingsManager);
    }

    public void GoToMainMenu()
    {
        dialogBox.Show(goToMainMenuContent);
        dialogBox.OnPositiveButtonClicked += () =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Main Menu");
        };
    }

    public void Quit()
    {
        dialogBox.Show(quitContent);
        dialogBox.OnPositiveButtonClicked += () =>
        {
            Application.Quit();
        };
    }

    private void ShowPauseMenu()
    {
        Time.timeScale = 0;
        _paused = true;

        menuUI.Show();

        onPause?.Invoke();
    }


    private void HidePauseMenu()
    {
        Time.timeScale = 1;
        _paused = false;

        menuUI.Hide();

        onUnpause?.Invoke();
    }

    protected override void OnBack() => HidePauseMenu();
}