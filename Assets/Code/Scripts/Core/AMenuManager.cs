using UnityEngine;

public abstract class AMenuManager : MonoBehaviour 
{
    protected IMenuComponent currentMenuComponent;

    protected void RegisterMenuComponent(IMenuComponent menuComponent)
    {
        if (currentMenuComponent != null)
            currentMenuComponent.Back();

        currentMenuComponent = menuComponent;
        currentMenuComponent.Show();
    }

    public void Back()
    {
        if (currentMenuComponent == null)
            OnBack();
        else if(currentMenuComponent.CanGoBack())
        {
            currentMenuComponent.Back();
            currentMenuComponent = null;
        }

    }

    protected abstract void OnBack();
}
