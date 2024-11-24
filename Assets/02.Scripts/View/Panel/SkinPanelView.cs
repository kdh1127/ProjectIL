using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinPanelView : MonoBehaviour
{
    public Transform content_tr;
    public GameObject skinItem;
    public ToggleGroup toggleGroup;

    public void RegisterToggle(Toggle toggle)
    {
        toggleGroup.RegisterToggle(toggle);
    }

}
