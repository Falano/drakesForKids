using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WindowManager: MonoBehaviour, IWindowManager
{
    //[SerializeField] protected Window Type;
    [SerializeField] bool isActiveAtStart;

    public void Start()
    {
        gameObject.SetActive(isActiveAtStart);
    }

    public void Activate()
    {
        if(WindowsManager.instance.ActiveWindow == null)
        {
            Debug.LogError("No active window! Probably hasn't been initialized");
        }
        //WindowsManager.instance.ActiveWindow.Deactivate();
        //WindowsManager.instance.ActiveWindow = this;
        // enable and raise the menu
        gameObject.SetActive(true); // tmp
    }

    public void Deactivate()
    {
        // lower and disable the menu
        gameObject.SetActive(false); // tmp
    }
}