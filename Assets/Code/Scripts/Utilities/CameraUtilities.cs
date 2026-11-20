using UnityEngine;

public static class CameraUtilities
{
    private static Camera m_mainCamera;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        m_mainCamera = Camera.main;
    }

    public static Vector3 GetRotationAxisFromDirection(Vector2 direction)
        => m_mainCamera.transform.right * direction.x + m_mainCamera.transform.up * direction.y;

    public static Quaternion GetCameraRelativeRotation(Quaternion localRotation) 
        => Quaternion.Inverse(m_mainCamera.transform.rotation) * localRotation;
    public static Vector3 GetCameraRelativeRotationEuler(Quaternion localRotation) 
        => GetCameraRelativeRotation(localRotation).eulerAngles;
}
