using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Goer : MonoBehaviour, IPointerClickHandler
{
    public WindowManager Goal;

    public static void GoToWindow(WindowManager goal)
    {
        //WindowsManager.instance.Windows[WindowsManager.instance.ActiveWindow].Deactivate();
        Debug.Log("Going to " + goal.ToString());
        WindowsManager.instance.ActiveWindow.Deactivate();
        WindowsManager.instance.ActiveWindow = goal;
        WindowsManager.instance.ActiveWindow.Activate();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GoToWindow(Goal);
    }
}
