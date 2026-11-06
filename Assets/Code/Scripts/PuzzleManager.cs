using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private CubeController cubeController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private EnvironmentManager environmentManager;
    [SerializeField] private MenuInputController menuInputController;
    [SerializeField] private GameWonManager gameWonManager;

    private GameStats _gameStats;

    private void Start()
    {
        _gameStats = new GameStats()
        {
            size = GameRequest.cubeSize
        };
        
        cubeController.Initialize();
        environmentManager.Initialize();

        cameraController.ZoomMin = cubeController.Size;
        cameraController.ZoomMax = cubeController.Size * 4;

        cubeController.onMove += () => _gameStats.moves++;
        cubeController.onCubeSolved += OnCubeSolved;

        TimeCounter.ResetTimer();
    }

    private void OnCubeSolved()
    {
        TimeCounter.StopCounting();
        _gameStats.time = TimeCounter.GetTime();
        
        gameWonManager.DisplayStats(_gameStats);
        gameWonManager.Show();

        menuInputController.DisableInput();
    }
}
