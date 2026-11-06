using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private SkyboxReference skyboxReference;

    public void Initialize()
    {
        RenderSettings.skybox = skyboxReference.Reference[GameRequest.environmentIndex].material;
    }
}
