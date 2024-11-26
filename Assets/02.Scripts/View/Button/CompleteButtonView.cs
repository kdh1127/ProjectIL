using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteButtonView : MonoBehaviour
{
    public Button button;
   
    public void Init(bool isInteractable)
    {
        button.interactable = isInteractable;
    }
    public void SetInteractable(bool isInteractable)
    {
        button.interactable = isInteractable;
    }

}
