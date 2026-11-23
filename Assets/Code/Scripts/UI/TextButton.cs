using DG.Tweening;
using SoundSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextButton : Selectable
{
    [Space(20)]
    [SerializeField] private string text;
    [SerializeField] protected Color baseColor;
    [SerializeField] private float hoveredWidth;
    [SerializeField] protected TextMeshProUGUI tmp;
    [SerializeField] private AudioResource hoverSound;
    [SerializeField] private AudioResource pressSound;
    [SerializeField] protected string defaultFormat = "%T";
    [SerializeField] protected string hoveredFormat = "%T";

    private RectTransform _rectTransform;
    private float _baseWidth;
    private float _baseHeight;

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
        baseColor.a = 0.7f;
        if(tmp) tmp.color = baseColor;
        _baseWidth = _rectTransform.sizeDelta.x;
        _baseHeight = _rectTransform.sizeDelta.y;
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

        AudioManager.PlayAudio(pressSound, MixerGroups.UI);
    }
    protected void ReformatText(string format) => SetText(format.Replace("%T", text));

    public override void OnPointerEnter(PointerEventData eventData)
    {
        _rectTransform.DOSizeDelta(new Vector2(hoveredWidth, _baseHeight), 0.5f).SetUpdate(true);
        tmp.DOFade(1, 0.5f).SetUpdate(true);

        ReformatText(hoveredFormat);

        AudioManager.PlayAudio(hoverSound, MixerGroups.UI);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        _rectTransform.DOSizeDelta(new Vector2(_baseWidth, _baseHeight), 0.5f).SetUpdate(true);
        tmp.DOFade(0.7f, 0.5f).SetUpdate(true);

        ReformatText(defaultFormat);
    }

}
