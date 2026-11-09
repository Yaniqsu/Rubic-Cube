using System.Collections;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private CubeController cubeController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private EnvironmentManager environmentManager;
    [SerializeField] private MenuInputController menuInputController;
    [SerializeField] private GameWonManager gameWonManager;
    [SerializeField] private SpeedRunStatsManager speedRunStatsManager;

    private GameStats _gameStats;

    private void Start()
    {
        _gameStats = new GameStats()
        {
            size = GameRequest.cubeSize
        };

        StartCoroutine(DelayInitialization());
    }

    private IEnumerator DelayInitialization()
    {
        yield return null;

        Initialize();
    }
    
    private void Initialize()
    {
        cubeController.Initialize(GameRequest.randomizeTurns);
        environmentManager.Initialize();

        cameraController.ZoomMin = cubeController.Size;
        cameraController.ZoomMax = cubeController.Size * 4;

        cubeController.onMove += () => _gameStats.moves.Value++;
        cubeController.onUndo += () => _gameStats.moves.Value--;
        cubeController.onCubeSolved += OnCubeSolved;

        if (GameRequest.speedunMode) speedRunStatsManager.Initialize(_gameStats.moves);

        TimeCounter.ResetTimer();
    }

    private void OnCubeSolved()
    {
        TimeCounter.StopCounting();
        _gameStats.time.Value = TimeCounter.GetTime();

        speedRunStatsManager.Hide();
        
        gameWonManager.DisplayStats(_gameStats);
        gameWonManager.Show();

        menuInputController.DisableInput();
    }
}
