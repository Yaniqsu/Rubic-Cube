using DG.Tweening;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform leftSection;
    [SerializeField] private RectTransform rightSection;
    [SerializeField] private CanvasGroup pauseMenuCanvasGroup;

    private void Start()
    {
        leftSection.anchoredPosition = new Vector2(-100, 0);
        rightSection.anchoredPosition = new Vector2(100, 0);
        pauseMenuCanvasGroup.alpha = 0;
    }

    public void ToggleCanvas(bool active) => canvas.enabled = active;

    public void Show()
    {
        ToggleCanvas(true);

        leftSection.DOAnchorPosX(0, 0.5f).SetUpdate(true);
        rightSection.DOAnchorPosX(0, 0.5f).SetUpdate(true);
        pauseMenuCanvasGroup.DOFade(1, 0.5f).SetUpdate(true);
    }

    public void Hide()
    {
        leftSection.DOAnchorPosX(-100, 0.5f).SetUpdate(true).onComplete += () => ToggleCanvas(true); ;
        rightSection.DOAnchorPosX(100, 0.5f).SetUpdate(true);
        pauseMenuCanvasGroup.DOFade(0, 0.5f).SetUpdate(true);
    }
}
