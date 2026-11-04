using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private CubeController cubeController;
    [SerializeField] private CameraController cameraController;

    private void Start()
    {
        cameraController.ZoomMin = cubeController.Size;
        cameraController.ZoomMax = cubeController.Size * 3;

        cubeController.Initialize();
    }
}
