using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    [SerializeField] private CubeInputController cubeInputController;
    [SerializeField] private CubeController cubeController;

    private void Start()
    {
        PauseMenuManager.onPause += DisableInput;
        PauseMenuManager.onUnpause += EnableInput;

        cubeController.onCubeSolved += DisableInput;
    }

    private void OnDestroy()
    {
        PauseMenuManager.onPause -= DisableInput;
        PauseMenuManager.onUnpause -= EnableInput;
    }

    private void EnableInput()
    {
        inputController.EnableInput();
        cubeInputController.EnableInput();
    }

    private void DisableInput()
    {
        inputController.DisableInput();
        cubeInputController.DisableInput();
    }
}
