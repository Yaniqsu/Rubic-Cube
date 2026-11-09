using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SpeedRunStatsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI movesText;
    [SerializeField] private RectTransform topSection;
    [SerializeField] private CanvasGroup canvasGroup;

    public void Initialize(EventData<int> movesData)
    {
        SetMoves(0);
        SetTime(0);
        
        movesData.OnValueChanged += SetMoves;

        TimeCounter.OnTick += SetTime;

        Show();
    }

    private void Show()
    {
        topSection.DOAnchorPosY(0, 0.5f);
        canvasGroup.DOFade(1, 0.5f);
    }

    public void Hide()
    {
        topSection.DOAnchorPosY(30, 0.5f);
        canvasGroup.DOFade(0, 0.5f);
    }

    private void SetTime(float seconds)
    {
        var timespan = TimeSpan.FromSeconds(seconds);
        var deltaTime = new DateTime(timespan.Ticks);

        timeText.text = $"Time: {deltaTime:mm:ss}";
    }

    private void SetMoves(int totalMoves)
    {
        movesText.text = $"Moves: {totalMoves}";
    }
}
