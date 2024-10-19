using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonToggle : Toggle
{
    public enum ToggleType
    {
        Quest,
        Weapon,
        Shop
    }

	private ToggleType type;
	public ToggleType Type { get => type; set => type = value; }
}