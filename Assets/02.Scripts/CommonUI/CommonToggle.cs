using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonToggle : Toggle
{
    [System.Serializable]
    public enum ToggleType
    {
        Quest=0,
        Weapon=1,
        Mission=2,
        Shop=3     
    }

	[SerializeField] public ToggleType type;
}