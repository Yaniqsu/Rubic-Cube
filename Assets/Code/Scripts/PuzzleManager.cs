using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private CubeController cubeController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private EnvironmentManager environmentManager;

    private void Start()
    {
        cubeController.Initialize();
        environmentManager.Initialize();

        cameraController.ZoomMin = cubeController.Size;
        cameraController.ZoomMax = cubeController.Size * 4;
    }
}
