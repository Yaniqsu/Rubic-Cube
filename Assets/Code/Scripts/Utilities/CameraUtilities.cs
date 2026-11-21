using UnityEngine;
using UnityEngine.SceneManagement;

public static class CameraUtilities
{
    private static Camera m_mainCamera;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        SceneManager.activeSceneChanged += ChangeMainCamera;
        Application.quitting += OnApplicationQuit;
    }

    private static void ChangeMainCamera(Scene scene1, Scene scene2)
    {
        m_mainCamera = Camera.main;
    }

    private static void OnApplicationQuit()
    {
        SceneManager.activeSceneChanged -= ChangeMainCamera;
    }

    public static Vector3 GetRotationAxisFromDirection(Vector2 direction)
        => m_mainCamera.transform.right * direction.x + m_mainCamera.transform.up * direction.y;

    public static Quaternion GetCameraRelativeRotation(Quaternion localRotation) 
        => Quaternion.Inverse(m_mainCamera.transform.rotation) * localRotation;
    public static Vector3 GetCameraRelativeRotationEuler(Quaternion localRotation) 
        => GetCameraRelativeRotation(localRotation).eulerAngles;
}
