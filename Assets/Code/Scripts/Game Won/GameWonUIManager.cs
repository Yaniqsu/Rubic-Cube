using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameWonUIManager : MonoBehaviour 
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup mainCanvasGroup;
    [SerializeField] private RectTransform header;
    [SerializeField] private RectTransform header2;
    [SerializeField] private TextMeshProUGUI headerTmp;
    [SerializeField] private TextMeshProUGUI header2Tmp;
    [SerializeField] private RectTransform bottomSection;
    [SerializeField] private CanvasGroup bottomSectionCanvasGroup;
    [SerializeField] private TextMeshProUGUI summaryText;

    private float _headerStartPosY;
    private float _header2StartPosY;
    private bool _active;

    private void Start()
    {
        _headerStartPosY = header.anchoredPosition.y;
        _header2StartPosY = header2.anchoredPosition.y;

        headerTmp.DOFade(0, 0);
        header2Tmp.DOFade(0, 0);

        bottomSectionCanvasGroup.alpha = 0;

        bottomSection.anchoredPosition = new Vector2(0, -100);
        header.anchoredPosition = new Vector2(0, _headerStartPosY + 20);
        header2.anchoredPosition = new Vector2(0, _header2StartPosY + 20);
    }

    public void Show()
    {
        if (_active)
            return;
        
        _active = true;
        canvas.enabled = true;

        StartCoroutine(ShowSequence());
    }

    public void DisplayStats(GameStats stats)
    {
        var timespan = TimeSpan.FromSeconds(stats.time.Value);
        var dateTime = new DateTime(timespan.Ticks);

        summaryText.text = "" +
            $"Cube Size: {stats.size}\n" +
            $"Time: {dateTime:mm:ss}\n" +
            $"Total moves: {stats.moves.Value}";
    }

    private IEnumerator ShowSequence()
    {
        header.DOAnchorPosY(_headerStartPosY, 1);
        headerTmp.DOFade(1, 1);

        yield return new WaitForSeconds(0.5f);

        header2.DOAnchorPosY(_header2StartPosY, 1);
        header2Tmp.DOFade(1, 1);

        yield return new WaitForSeconds(0.5f);

        bottomSection.DOAnchorPosY(0, 1);
        bottomSectionCanvasGroup.DOFade(1, 1);

        mainCanvasGroup.interactable = true;
    }
}
