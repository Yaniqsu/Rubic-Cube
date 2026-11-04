using UnityEngine;

public class MainMenuCube : MonoBehaviour
{
    [SerializeField] private Vector3 axis;
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Rotate(speed * Time.deltaTime * axis);
    }
}
