using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage;

public class TRPopup : MonoBehaviour
{
    public virtual void Open()
    {
        //PopupManager.Instance.AddPopup(this);
    }

    public virtual void Close()
    {
        //PopupManager.Instance.RemovePopup(this);
    }
}
