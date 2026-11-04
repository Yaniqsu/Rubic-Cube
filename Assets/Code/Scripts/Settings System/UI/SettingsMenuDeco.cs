using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SettingsMenuDeco : MonoBehaviour
{
    [SerializeField] private string coordFormat = "coord: %x x %y";
    [SerializeField] private string clockFormat = "time: %t";
    [SerializeField] private string randomFormat = "random_value: %r";
    [SerializeField] private TextMeshProUGUI coordText;
    [SerializeField] private TextMeshProUGUI clockText;
    [SerializeField] private TextMeshProUGUI randomText;

    private Coroutine clockRoutine;
    private Coroutine randomRoutine;

    private void Update()
    {
        var pos = Input.mousePosition;

        coordText.text = coordFormat
            .Replace("%x", Math.Round(pos.x, 2).ToString())
            .Replace("%y", Math.Round(pos.y, 2).ToString());

    }

    private void OnEnable()
    {
        clockRoutine = StartCoroutine(ClockRoutine());
        randomRoutine = StartCoroutine(RandomRoutine());
    }

    private void OnDisable()
    {
        StopCoroutine(clockRoutine);
        StopCoroutine(randomRoutine);
    }

    private IEnumerator ClockRoutine()
    {
        var wait = new WaitForSecondsRealtime(1);

        while (true)
        {
            clockText.text = clockFormat.Replace("%t", $"{DateTime.Now:HH:mm:ss}]");

            yield return wait;
        }
    }    

    private IEnumerator RandomRoutine()
    {
        while(true)
        {
            randomText.text = randomFormat
                .Replace("%r", UnityEngine.Random.Range(1000, 9999).ToString());

            yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(0.5f, 2));
        }
    }
}
