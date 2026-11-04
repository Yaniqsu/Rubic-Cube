using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [field: SerializeField] public float ZoomMin { get; set; }
    [field: SerializeField] public float ZoomMax {get; set; }

    public void Zoom(float delta)
    {
        var zPos = Mathf.Clamp(cameraTransform.localPosition.z + delta, -ZoomMax, -ZoomMin);
        
        cameraTransform.localPosition = new Vector3 (0, 0, zPos);
    }
}
