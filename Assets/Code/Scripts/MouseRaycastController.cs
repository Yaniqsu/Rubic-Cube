using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MouseRaycastController : MonoBehaviour 
{
    [SerializeField] private LayerMask targetLayer;
    [Space]
    [SerializeField] private bool startAutomatically;
    [SerializeField] private float interval;
    
    private Camera _main;
    private WaitForSeconds _wait;
    private Coroutine _checkRoutine;

    public bool HitLastCheck { get; private set; }
    public RaycastHit LastHitInfo { get; private set; }

    public UnityEvent<bool> OnCheck;

    private void Awake()
    {
        _main = Camera.main;
        _wait = new WaitForSeconds(interval);
    }

    private void Start()
    {
        if (startAutomatically)
            StartChecking();
    }

    public void StartChecking()
    {
        if(_checkRoutine == null)
            _checkRoutine = StartCoroutine(RaycastRoutine());
    }

    public void StopChecking()
    {
        if (_checkRoutine != null)
            StopCoroutine(_checkRoutine);
    }

    private IEnumerator RaycastRoutine()
    {
        while(true)
        {
            DoRaycast();

            yield return _wait;
        }
    }

    private void DoRaycast()
    {
        var ray = _main.ScreenPointToRay(Input.mousePosition);
        HitLastCheck = Physics.Raycast(ray, out var hitInfo, float.PositiveInfinity, targetLayer);

        if(HitLastCheck)
        {
            LastHitInfo = hitInfo;
        }

        OnCheck.Invoke(HitLastCheck);
    }
}
