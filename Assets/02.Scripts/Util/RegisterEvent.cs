using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RegisterEvent : MonoBehaviour
{
    [SerializeField]
    public UnityEvent action;

    public void DoAction(string name)
    {
        for (int i = 0; i < action.GetPersistentEventCount(); i++)
        if (action.GetPersistentMethodName(i) == name)
        {
            action?.Invoke();
        }
    }
}
