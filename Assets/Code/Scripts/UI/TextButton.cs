using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextButton : Selectable
{
    [SerializeField] private string text;
    [SerializeField] protected Color baseColor;
    [SerializeField] private float hoveredWidth;
    [SerializeField] protected TextMeshProUGUI tmp;
    [SerializeField] protected string defaultFormat = "%T";
    [SerializeField] protected string hoveredFormat = "%T";

    private RectTransform _rectTransform;
    private float _baseWidth;

    public string Text
    {
        set 
        {
            text = value;
            SetText(value); 
        }
    }
    public UnityEvent onClick;

    protected override void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        baseColor.a = 0.8f;
        tmp.color = baseColor;
        _baseWidth = _rectTransform.sizeDelta.x;
        SetText(text);
    }

    protected override void OnValidate()
    {
        gameObject.name = $"Button: {text}";
        SetText(text);
        SetColor(baseColor);
    }

    private void SetText(string text)
    {
        if (!tmp)
            return;

        tmp.text = text;
    }

    protected void SetColor(Color color)
    {
        if (!tmp)
            return;

        color.a = tmp.color.a;
        tmp.color = color;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        onClick.Invoke();
    }
    protected void ReformatText(string format) => SetText(format.Replace("%T", text));

    public override void OnPointerEnter(PointerEventData eventData)
    {
        _rectTransform.DOSizeDelta(new Vector2(hoveredWidth, 30), 0.5f);
        tmp.DOFade(1, 0.5f);

        ReformatText(hoveredFormat);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        _rectTransform.DOSizeDelta(new Vector2(_baseWidth, 30), 0.5f);
        tmp.DOFade(0.8f, 0.5f);

        ReformatText(defaultFormat);
    }

}
