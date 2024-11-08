using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteButtonView : MonoBehaviour
{
    public Image complete_img;
    public Button button;
   
    public void Init(bool isClear)
    {
        button.interactable = isClear;
        complete_img.color = isClear ? new Color(255, 255, 255, 255) : new Color(60, 60, 60, 190);
    }
    public void SetInteractable(bool isInteractable)
    {
        var colorResources = TRScriptableManager.Instance.GetColor("InteractableColor").colorDictionary;

        button.interactable = isInteractable;
        complete_img.color = isInteractable ? colorResources["Enable"] : colorResources["Disable"];
    }

}
