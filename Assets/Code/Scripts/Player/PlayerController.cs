using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    [SerializeField] private CubeController cubeController;

    private void Start()
    {
        PauseMenuManager.onPause += inputController.DisableInput;
        PauseMenuManager.onUnpause += inputController.EnableInput;

        cubeController.onCubeSolved += inputController.DisableInput;
    }

    private void OnDestroy()
    {
        PauseMenuManager.onPause -= inputController.DisableInput;
        PauseMenuManager.onUnpause -= inputController.EnableInput;
    }
}
