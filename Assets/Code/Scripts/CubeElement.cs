using UnityEngine;

public class CubeElement : MonoBehaviour
{
    private bool _topWallEnabled;
    private bool _bottomWallEnabled;
    private bool _rightWallEnabled;
    private bool _leftWallEnabled;
    private bool _frontWallEnabled;
    private bool _backWallEnabled;
    
    [SerializeField] private GameObject wallTop;
    [SerializeField] private GameObject wallBottom;
    [SerializeField] private GameObject wallRight;
    [SerializeField] private GameObject wallLeft;
    [SerializeField] private GameObject wallFront;
    [SerializeField] private GameObject wallBack;

    public bool EnableTopWall
    {
        get => _topWallEnabled;
        set
        {
            _topWallEnabled = value;
            wallTop.SetActive(value);
        }
    }

    public bool EnableBottomWall
    {
        get => _bottomWallEnabled;
        set
        {
            _bottomWallEnabled = value;
            wallBottom.SetActive(value);
        }
    }

    public bool EnableRightWall
    {
        get => _rightWallEnabled;
        set
        {
            _rightWallEnabled = value;
            wallRight.SetActive(value);
        }
    }

    public bool EnableLeftWall
    {
        get => _leftWallEnabled;
        set
        {
            _leftWallEnabled = value;
            wallLeft.SetActive(value);
        }
    }

    public bool EnableFrontWall
    {
        get => _frontWallEnabled;
        set
        {
            _frontWallEnabled = value;
            wallFront.SetActive(value);
        }
    }

    public bool EnableBackWall
    {
        get => _backWallEnabled;
        set
        {
            _backWallEnabled = value;
            wallBack.SetActive(value);
        }
    }
}
