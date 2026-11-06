using UnityEngine;

public interface IMenuComponent
{
    public void Back();
    public void Show();

    public bool CanGoBack();
}
