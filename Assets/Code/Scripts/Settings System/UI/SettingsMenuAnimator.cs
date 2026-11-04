using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace YNQ.Settings.UI
{
    public class SettingsMenuAnimator : MonoBehaviour
	{
        [System.Serializable]
        private struct Anchor
        {
            [SerializeField] private Vector2 min;
            [SerializeField] private Vector2 max;

            public float MinX => min.x;
            public float MinY => min.y;
            public float MaxX => max.x;
            public float MaxY => max.y;

            public Vector2 Min => min;
            public Vector2 Max => max;
        }

        [Header("Values")]
        [SerializeField] private Anchor startAnchor;
        [SerializeField] private Anchor[] showAnchorSequence;
        [SerializeField] private Anchor[] hideAnchorSequence;
        [SerializeField] private string[] showTextSequence;
        [SerializeField] private float sequenceFrameDuration = 0.1f;
        [SerializeField] private float showAnimDuration = 0.4f;
        [SerializeField] private float backgroundFadeOutDuration = 0.5f;
        [SerializeField] private float hideAnimDuration = 0.4f;
        
        [Header("Components")]
        [SerializeField] private RectTransform settingsObject;
        [SerializeField] private RectTransform loadingBox;
        [SerializeField] private Image loadingImage;
        [SerializeField] private TextMeshProUGUI loadngText;
        [SerializeField] private CanvasGroup loadingCanvasGroup;
        [SerializeField] private CanvasGroup settingsCanvasGroup;

        public UnityEvent onShowAnimationEnd;
        public UnityEvent onBackAnimationEnd;
        private Coroutine textSequenceRoutine;
        private Coroutine _animationRoutine;

        public bool Animating { get; private set; }

        public void PlayShowAnimation()
        {
            settingsObject.gameObject.SetActive(true);
            
            loadingBox.anchorMin = startAnchor.Min;
            loadingBox.anchorMax = startAnchor.Max;
            
            RegisterAnimation(ShowAnimation());
            textSequenceRoutine = StartCoroutine(TextSequence());
        }

        public void PlayHideAnimation()
        {
            RegisterAnimation(HideAnimation());
        }

        private IEnumerator TextSequence()
        {
            loadngText.DOFade(1, 0).SetUpdate(true);

            for (var i = 0; i < showTextSequence.Length; i++)
            {
                loadngText.text = showTextSequence[i];
                yield return new WaitForSecondsRealtime(sequenceFrameDuration);
            }
        }

        private IEnumerator ShowAnimation()
        {
            loadingImage.DOFade(1, 0).SetUpdate(true);
            loadingCanvasGroup.blocksRaycasts = true;
            settingsCanvasGroup.alpha = 0;
            var showFrameDuration = showAnimDuration / showAnchorSequence.Length;

            for(var i = 0; i < showAnchorSequence.Length; i++)
            {
                loadingBox.DOAnchorMin(showAnchorSequence[i].Min, showFrameDuration)
                .SetEase(Ease.OutSine)
                .SetUpdate(true);

                loadingBox.DOAnchorMax(showAnchorSequence[i].Max, showFrameDuration)
                    .SetEase(Ease.OutSine)
                    .SetUpdate(true);

                yield return new WaitForSecondsRealtime(showFrameDuration);
            }

            onShowAnimationEnd.Invoke();
            settingsCanvasGroup.alpha = 1;

            loadingImage.DOFade(0, backgroundFadeOutDuration)
                .SetUpdate(true);

            loadngText.DOFade(0, showFrameDuration)
                .SetUpdate(true);

            yield return new WaitForSecondsRealtime(backgroundFadeOutDuration);

            loadingCanvasGroup.blocksRaycasts = false;

        }

        private IEnumerator HideAnimation()
        {
            var hideFrameDuration = hideAnimDuration / hideAnchorSequence.Length;
            
            loadingCanvasGroup.blocksRaycasts = true;

            loadingImage.DOFade(1, 0f)
                .SetUpdate(true);

            settingsCanvasGroup.alpha = 0;

            for (var i = 0; i < hideAnchorSequence.Length; i++)
            {
                loadingBox.DOAnchorMin(hideAnchorSequence[i].Min, hideFrameDuration)
                .SetEase(Ease.OutSine)
                .SetUpdate(true);

                loadingBox.DOAnchorMax(hideAnchorSequence[i].Max, hideFrameDuration)
                    .SetEase(Ease.OutSine)
                    .SetUpdate(true);

                yield return new WaitForSecondsRealtime(hideFrameDuration);
            }

            loadingImage.DOFade(1, hideAnimDuration)
                .SetUpdate(true);

            loadingCanvasGroup.blocksRaycasts = false;
            
            yield return new WaitForSecondsRealtime(hideAnimDuration);

            settingsObject.gameObject.SetActive(false);

            onBackAnimationEnd.Invoke();
        }

        private void RegisterAnimation(IEnumerator animation)
        {
            if (_animationRoutine != null)
                StopCoroutine(_animationRoutine);
            
            _animationRoutine = StartCoroutine(PlayAnimation(animation));
        }
        
        private IEnumerator PlayAnimation(IEnumerator animation)
        {
            Animating = true;
            
            yield return animation;

            Animating = false;
        }
    } 
}
