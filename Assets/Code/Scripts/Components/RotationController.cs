using System.Collections;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float startingTime;

    private Quaternion _rotationQuaternion;
    private bool _active;

    private void Awake()
    {
        _rotationQuaternion = Quaternion.Euler(Vector3.zero);
    }

    private void Update()
    {
        if(_active)
        {
            transform.rotation *= _rotationQuaternion;
        }
    }

    public void StartRotation()
    {
        _active = true;
        StartCoroutine(ChangeRotation(rotation, startingTime));
    }

    public void StopRotation()
    {
        _active = false;
    }

    private IEnumerator ChangeRotation(Vector3 rotation, float time)
    {
        var currentRotation = _rotationQuaternion;
        var targetRotation = Quaternion.Euler(rotation);
        var elapsed = 0f;

        while (elapsed < time)
        {
            _rotationQuaternion = Quaternion.Slerp(currentRotation, targetRotation, elapsed / time);

            elapsed += Time.deltaTime;
            yield return null;
        }

        _rotationQuaternion = targetRotation;
    }
}
