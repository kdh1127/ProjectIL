using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage.DesignPattern;

public class GameManager : TRSingleton<GameManager>
{
	private new void Awake()
	{
		base.Awake();
        DOTween.Init();
	}
}
