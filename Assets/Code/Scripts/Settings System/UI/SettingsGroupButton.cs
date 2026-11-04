using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsGroupButton : TextButton
{
    private bool _selected;

    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            ReformatText(_selected ? hoveredFormat : defaultFormat);
            var startColor = _selected ? baseColor * 1.5f : tmp.color;
            var endColor = baseColor * (_selected ? colors.selectedColor : colors.normalColor);
            AnimateColor(startColor, endColor, 0.6f);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        SetColor(baseColor * colors.normalColor);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(!Selected)
            base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if(!Selected)
            base.OnPointerExit(eventData);
    }

    private void AnimateColor(Color startColor, Color endColor, float time)
    {
        SetColor(startColor);
        tmp.DOKill();
        tmp.DOColor(endColor, time)
            .SetUpdate(true);
    }
}
