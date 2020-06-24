using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TemperatureModifier : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int value;
    private Image img;
    private Color col;

    private void Start()
    {
        img = GetComponent<Image>();
        col = img.color;
    }

    #region pointerEvents
    public void OnPointerClick(PointerEventData eventData)
    {
        WindowsManager.instance.HatchSceneManager.ChangeTemperature(value);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // image change or emitPulse or smth
        img.color = Color.white; // tmp

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // end image change smth
        img.color = col; // tmp
    }
    #endregion pointerEvents
}
