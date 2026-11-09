using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SceneSwitcher : MonoBehaviour
{
    private struct SceneLoadRequest
    {
        public int sceneIndex;
        public string sceneName;
        public bool useID;
        public bool useName;

        public SceneLoadRequest(int sceneID)
        {
            sceneIndex = sceneID;
            sceneName = string.Empty;
            useID = true;
            useName = false;
        }

        public SceneLoadRequest(string sceneName)
        {
            sceneIndex = -1;
            this.sceneName = sceneName;
            useID = false;
            useName = true;
        }
    }

    [SerializeField] private Image _blackout;
    [SerializeField] private Animator _introAnimator;
    [SerializeField] private CanvasGroup _loadingScreen;
    [SerializeField] private AudioResource _mapSound;


    private static SceneSwitcher _instance;
    private bool _switching;

    private const float FADE_IN_DURATION = 0.5f;
    private const float FADE_OUT_DURATION = 0.5f;
    private const float LOADING_DURATION = 1.5f;

    public static SceneSwitcher Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Instantiate(Resources.Load<SceneSwitcher>("SceneSwitcher"));
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    public static void SwitchScene(string sceneName, bool playMapAnimation = false, bool openMap = true)
    {
        if (Instance == null || Instance._switching)
            return;

        Instance._switching = true;

        Instance._blackout.DOFade(1, FADE_IN_DURATION)
            .SetUpdate(true)
            .onComplete = () => Instance.StartCoroutine(
                playMapAnimation
                    ? HandleSceneTransitionWithAnimation(new SceneLoadRequest(sceneName), openMap)
                    : HandleSceneTransition(new SceneLoadRequest(sceneName))
            );
    }

    public static void SwitchScene(int sceneID)
    {
        if (Instance == null || Instance._switching)
            return;

        Instance._switching = true;

        Instance._blackout.DOFade(1, FADE_IN_DURATION)
            .SetUpdate(true)
            .onComplete = () => Instance.StartCoroutine(HandleSceneTransition(new SceneLoadRequest(sceneID)));
    }

    private static IEnumerator HandleSceneTransition(SceneLoadRequest request)
    {
        var operation = request.useID
            ? SceneManager.LoadSceneAsync(request.sceneIndex)
            : SceneManager.LoadSceneAsync(request.sceneName);

        if (operation == null)
            yield break;

        yield return new WaitUntil(() => operation.isDone);

        Instance._loadingScreen.alpha = 1f;
        yield return new WaitForSeconds(LOADING_DURATION);
        Instance._loadingScreen.alpha = 0f;

        Time.timeScale = 1;
        Instance._switching = false;

        Instance._blackout.DOFade(0, FADE_OUT_DURATION)
            .SetUpdate(UpdateType.Fixed);
    }

    private static IEnumerator HandleSceneTransitionWithAnimation(SceneLoadRequest request, bool openMap)
    {
        yield return Instance.StartCoroutine(PlayIntroAnimation(openMap));

        var operation = request.useID
            ? SceneManager.LoadSceneAsync(request.sceneIndex)
            : SceneManager.LoadSceneAsync(request.sceneName);

        if (operation == null)
            yield break;

        yield return new WaitUntil(() => operation.isDone);

        if (!openMap)
        {
            Instance._loadingScreen.alpha = 1f;
            yield return new WaitForSeconds(LOADING_DURATION);
            Instance._loadingScreen.alpha = 0f;
        }

        Time.timeScale = 1;
        Instance._switching = false;
        Instance._introAnimator.gameObject.SetActive(false);

        Instance._blackout.DOFade(0, FADE_OUT_DURATION)
            .SetUpdate(UpdateType.Fixed)
            .OnComplete(() =>
            {
                //PauseMenuManager.CanBePaused = true;
            });
    }

    public static IEnumerator PlayIntroAnimation(bool expandMap)
    {
        Instance._introAnimator.gameObject.SetActive(true);
        Instance._introAnimator.Play("MapIntro", 0, expandMap ? 1f : 0f);
        Instance._introAnimator.SetFloat("speed", expandMap ? -1f : 1f);
        yield return null;

        while (true)
        {
            var state = Instance._introAnimator.GetCurrentAnimatorStateInfo(0);
            float time = state.normalizedTime;

            if (expandMap ? time <= 0f : time >= 1f)
                break;

            yield return null;
        }

        Instance._introAnimator.gameObject.SetActive(false);
    }
}