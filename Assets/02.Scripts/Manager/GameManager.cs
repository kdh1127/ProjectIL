using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage.DesignPattern;
using System.Numerics;

public class GameManager : TRSingleton<GameManager>
{
	private new void Awake()
	{
		base.Awake();
        DOTween.Init();
		RegisterCurrency();

	}

	public void RegisterCurrency()
	{
		CurrencyManager<Gold>.RegisterCurrency(ECurrencyType.GOLD, new Gold());
		CurrencyManager<Dia>.RegisterCurrency(ECurrencyType.DIA, new Dia());
		CurrencyManager<Key>.RegisterCurrency(ECurrencyType.KEY, new Key());
	}
}
