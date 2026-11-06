using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private CubeController cubeController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private EnvironmentManager environmentManager;
    [SerializeField] private MenuInputController menuInputController;
    [SerializeField] private GameWonManager gameWonManager;

    private void Start()
    {
        cubeController.Initialize();
        environmentManager.Initialize();

        cameraController.ZoomMin = cubeController.Size;
        cameraController.ZoomMax = cubeController.Size * 4;

        cubeController.onCubeSolved += OnCubeSolved;
    }

    private void OnCubeSolved()
    {
        gameWonManager.Show();
        menuInputController.DisableInput();
    }
}
