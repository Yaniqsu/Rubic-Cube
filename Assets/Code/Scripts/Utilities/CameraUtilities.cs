using UnityEngine;
using UnityEngine.SceneManagement;

public static class CameraUtilities
{
    private static Camera m_mainCamera;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        ChangeMainCamera();
        
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        Application.quitting += OnApplicationQuit;
    }

    private static void OnActiveSceneChanged(Scene from, Scene to) => ChangeMainCamera();

    private static void ChangeMainCamera()
    {
        m_mainCamera = Camera.main;
    }

    private static void OnApplicationQuit()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    public static Vector3 GetRotationAxisFromDirection(Vector2 direction)
        => m_mainCamera.transform.right * direction.x + m_mainCamera.transform.up * direction.y;

    public static Quaternion GetCameraRelativeRotation(Quaternion localRotation) 
        => Quaternion.Inverse(m_mainCamera.transform.rotation) * localRotation;
    public static Vector3 GetCameraRelativeRotationEuler(Quaternion localRotation) 
        => GetCameraRelativeRotation(localRotation).eulerAngles;
}
