using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MenuTab : MonoBehaviour
{  
    [SerializeField] private GameObject tabContent;
    [SerializeField] public UnityEvent onShowTab;
    [SerializeField] public UnityEvent onHideTab;
    [SerializeField] public UnityEvent onBack;
    [SerializeField] public UnityEvent onResetTab;
    [Header("Settings")]
    [SerializeField] private bool _hideOnBack = true;
    [SerializeField] private bool _hideOnExit = true;
    private List<MenuTab> subTabs = new();
    private MenuTab parent;
    private GameObject currentSelectedGameObject;
    private bool isDisplayed = false;
    
    public bool IsDisplayed => isDisplayed;

    private void Awake()
    {
        // Debug.Log(transform.childCount);
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).TryGetComponent<MenuTab>(out var subTab))
                subTabs.Add(subTab);
        }
        transform.parent.TryGetComponent(out parent);
    }

    private void ShowTab()
    {
        isDisplayed = true;
        onShowTab.Invoke();
        tabContent.SetActive(true);
    }
    private void HideTab()
    {
        isDisplayed = false;

        onHideTab.Invoke();
        
        foreach (var tab in subTabs)
           tab.ResetTab();

        tabContent.SetActive(false);
    }
    public void ShowTabAt(int index)
    {
        if (index < 0 || index >= subTabs.Count)
            throw new System.ArgumentOutOfRangeException("Index must be equal or greater than 0 and less than sub tabs size");

        if(_hideOnExit)
            HideTab();
        subTabs[index].ShowTab();
    }
    private void ResetTab()
    {
        HideTab();
        
        foreach (var tab in subTabs)
            tab.HideTab();
    }
    public void Back(bool silently = false)
    {
        if (isDisplayed)
        {
            if(_hideOnBack)
                HideTab();

            if (silently)
                return;
            
            parent?.ShowTab();
            onBack.Invoke();
        }
        else 
        {
            foreach (var tab in subTabs)
                tab.Back();
        }

    }
    /// <summary>
    /// Checks if this tab or any of its sub tabs is displayed
    /// </summary>
    public bool IsTabActive()
    {
        return subTabs.Any(tab => tab.IsTabActive()) || isDisplayed;
    }

    private void OnDisable()
    {
        if (isDisplayed)
            Back(true);
    }
}
