using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [System.Serializable]
    private struct TimerEvent
    {
        [field: SerializeField] public float Delay { get; private set; }
        [field: SerializeField] public UnityEvent Event { get; private set; }
    }

    [SerializeField] private bool startImmediately = true;
    [SerializeField] private TimerEvent[] events;

    private void Start()
    {
        if (startImmediately)
            StartTimers();
    }

    public void StartTimers()
    {
        foreach(var e in events)
        {
            StartCoroutine(HandleTimer(e.Delay, e.Event));
        }
    }

    public void StopAllTimers() => StopAllCoroutines();

    private IEnumerator HandleTimer(float delay, UnityEvent timerEvent)
    {
        yield return new WaitForSeconds(delay);

        timerEvent.Invoke();
    }
}
