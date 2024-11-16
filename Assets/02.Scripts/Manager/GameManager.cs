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
		RegisterCurrency();

	}

	public void RegisterCurrency()
	{
		CurrencyManager<IRCurrencyBase>.RegisterCurrency(ECurrencyType.GOLD, new Gold());
		CurrencyManager<IRCurrencyBase>.RegisterCurrency(ECurrencyType.DIA, new Dia());
		CurrencyManager<IRCurrencyBase>.RegisterCurrency(ECurrencyType.KEY, new Key());

	}
}
