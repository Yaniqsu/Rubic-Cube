using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    private static TimeCounter Instance;
    private static float time;
    private static bool counting;
    
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (counting)
            time += Time.deltaTime;
    }

    public static void ResetTimer(float startTime = 0)
    {
        if (Instance == null)
            return;

        time = startTime;
        counting = true;
    }
    
    public static void StopCounting()
    {
        if (Instance == null)
            return;

        counting = false;
    }
    
    public static void ResumeCounting()
    {
        if (Instance == null)
            return;

        counting = true;
    }

    public static float GetTime() => time;
}
