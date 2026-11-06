using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputController inputController;

    private void Start()
    {
        PauseMenuManager.onPause += inputController.DisableInput;
        PauseMenuManager.onUnpause += inputController.EnableInput;
    }

    private void OnDestroy()
    {
        PauseMenuManager.onPause -= inputController.DisableInput;
        PauseMenuManager.onUnpause -= inputController.EnableInput;
    }
}
