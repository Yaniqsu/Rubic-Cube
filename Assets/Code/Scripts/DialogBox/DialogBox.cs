using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject blocker;
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private GameObject endButtons;
    [SerializeField] private TextButton nextButton;
    [SerializeField] private TextButton negativeButton;
    [SerializeField] private TextButton positiveButton;
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI contentText;

    [SerializeField] private float textAnimationDuration;

    private int _currentSlide;
    private Coroutine _animateTextRoutine;
    private string[] _slides;

    public event Action OnNegativeButtonClicked;
    public event Action OnPositiveButtonClicked;
    public event Action OnHide;

    public bool Active { get; private set; }

    private void Start()
    {
        dialogBox.SetActive(false);
        
        nextButton.onClick.AddListener(NextSlide);
        positiveButton.onClick.AddListener(PositiveButtonClicked);
        negativeButton.onClick.AddListener(NegativeButtonClicked);
    }

    public void Show(DialogContent dialogContent)
    {
        OnNegativeButtonClicked = null;
        OnPositiveButtonClicked = null;
        OnHide = null;

        Active = true;
        
        //negativeButton.GetComponent<ContentSizeFitter>().enabled = true;
        //positiveButton.GetComponent<ContentSizeFitter>().enabled = true;
        
        headerText.text = dialogContent.Header;
        nextButton.Text = dialogContent.NextButtonText;
        negativeButton.Text = dialogContent.NegativeButtonText;
        positiveButton.Text = dialogContent.PositiveButtonText;
        _slides = dialogContent.Slides;
        
        negativeButton.gameObject.SetActive(dialogContent.UseNegativeButton);
        
        _currentSlide = 0;
        dialogBox.SetActive(true);
        blocker.SetActive(true);
        
        ShowSlide();
    }

    private IEnumerator DisableContentSizeFitters()
    {
        yield return null;
        
        //negativeButton.GetComponent<ContentSizeFitter>().enabled = false;
        //positiveButton.GetComponent<ContentSizeFitter>().enabled = false;
    }

    private void Hide()
    {
        Active = false;
        
        dialogBox.SetActive(false);
        blocker.SetActive(false);
        OnHide?.Invoke();
    }

    private void PositiveButtonClicked()
    {
        OnPositiveButtonClicked?.Invoke();
        
        Hide();
    }

    private void NegativeButtonClicked()
    {
        OnNegativeButtonClicked?.Invoke();

        Hide();
    }

    private void ShowSlide()
    {
        if (_currentSlide == _slides.Length - 1)
        {
            nextButton.gameObject.SetActive(false);
            endButtons.SetActive(true);
            StartCoroutine(DisableContentSizeFitters());
        }
        else
        {
            nextButton.gameObject.SetActive(true);
            endButtons.SetActive(false);
        }
        
        if(_animateTextRoutine != null)
            StopCoroutine(_animateTextRoutine);
        
        _animateTextRoutine = StartCoroutine(AnimateText(_slides[_currentSlide]));
    }

    private void NextSlide()
    {
        _currentSlide++;
        ShowSlide();
    }

    private IEnumerator AnimateText(string text)
    {
        var wait = new WaitForSecondsRealtime(textAnimationDuration / text.Length);

        for (var i = 0; i < text.Length; i++)
        {
            contentText.text = text.Substring(0, i);

            yield return wait;
        }

        contentText.text = text;
    }
}
