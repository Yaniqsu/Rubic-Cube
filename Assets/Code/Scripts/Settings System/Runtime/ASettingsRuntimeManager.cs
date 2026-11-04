using UnityEngine;

public abstract class ASettingsRuntimeManager : MonoBehaviour
{
    private void OnEnable()
    {
        Connect();
    }

    private void OnDisable()
    {
        Disconnect();
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    protected abstract void Connect();
    protected abstract void Disconnect();   
}
