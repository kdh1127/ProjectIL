using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage.DesignPattern;

public class CharacterManager : TRSingleton<CharacterManager>
{
	[SerializeField] private Transform characterPosition_tr;

	public float CharacterPositionX { get => characterPosition_tr.localPosition.x; }

	private new void Awake()
	{
		base.Awake();
	}

	public void Init()
	{
		characterPosition_tr.localPosition = Vector3.zero;
	}

    public void ResetCharacter()
    {
		characterPosition_tr.localPosition = Vector3.zero;
	}
}
