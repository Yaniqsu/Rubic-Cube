using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YNQ.InputField;

public class SetupManager : MonoBehaviour, IMenuComponent
{
    [SerializeField] private Canvas setupCanvas;
    [SerializeField] private RectTransform setupObject;
    [SerializeField] private CanvasGroup objectCanvasGroup;
    [SerializeField] private Image blocker;
    [SerializeField] private SkyboxReference skyboxReference;
    [SerializeField] private DifficultyPresets difficultyPresets;
    [SerializeField] private GameObject solvedDifficultyInfo;

    [Space]
    [SerializeField] private InputField_Slider cubeSize;
    [SerializeField] private InputField_Switch speedrunMode;
    [SerializeField] private InputField_TextSpinner environment;
    [SerializeField] private InputField_TextSpinner difficultyPresetsInputField;

    private void Start()
    {
        environment.SetValues(skyboxReference.Reference.Select(r => r.name).ToArray());
        difficultyPresetsInputField.SetValues(difficultyPresets.GetNames());

        difficultyPresetsInputField.SetIndex(difficultyPresets.GetNames().Length - 1);
        difficultyPresetsInputField.OnValueChanged.AddListener(index => solvedDifficultyInfo.SetActive(index == 0));
    }


    public void Show()
    {
        setupCanvas.enabled = true;

        objectCanvasGroup.alpha = 0;
        blocker.DOFade(0, 0);
        setupObject.anchoredPosition = new Vector2(0, 100);

        objectCanvasGroup.DOFade(1, 0.5f);
        blocker.DOFade(0.4f, 0.5f);
        setupObject.DOAnchorPosY(0, 0.5f);

        cubeSize.SetValue(3);
    }

    public void Back()
    {
        objectCanvasGroup.DOFade(0, 0.5f);
        blocker.DOFade(0, 0.5f);
        setupObject.DOAnchorPosY(100, 0.5f).onComplete += () => setupCanvas.enabled = false;
    }

    public void Play()
    {
        GameRequest.cubeSize = cubeSize.IntValue;
        GameRequest.speedunMode = speedrunMode.Value;
        GameRequest.environmentIndex = environment.Value;
        GameRequest.randomizeTurns = difficultyPresets.GetTurns(difficultyPresetsInputField.Value);

        SceneManager.LoadScene("Game Scene");
    }

    public bool CanGoBack() => true;
}
