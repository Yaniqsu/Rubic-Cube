using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MenuInputController : AInputController
{
    [SerializeField] private string back;

    public UnityEvent onBack;
    
    protected override void EnableInputActions()
    {
        RegisterInputAction(back, _ => onBack.Invoke());
    }
}
