using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using ThreeRabbitPackage.DesignPattern;
using I2.Loc;
using AlphabetNumber;
using System.Numerics;
using System.Linq;

public class MainScenePresenter : TRSingleton<MainScenePresenter>
{
	public TopPanelView topPanelView;
	public BottomPanelView bottomPanelView;

	public MainButtonView mainButtonView;
	private MainButtonModel mainButtonModel = new();


	public TreasurePanelView treasurePanelView;
	private TreasureModel treasureModel = new();

	public CurrencyView currencyView;

	private void OnApplicationQuit()
	{
		//DataUtility.Save("WeaponModel", weaponModel);
	}
	private new void Awake()
	{
		base.Awake();
	}

	private void Start()
	{
		Subscribe();
	}

	private void Subscribe()
	{
		TopPanelSubscribe();
		MainButtonSubscribe();
		CurrencySubscribe();
	}

	private void TopPanelSubscribe()
	{
		StageManager.Instance.CurStage.Subscribe(curStage =>
		{
			topPanelView.stage_txt.text = StageManager.Instance.GetLocalizationStage(curStage);
		}).AddTo(this);
	}

	public void MainButtonSubscribe()
	{
		mainButtonModel.RegisterToggleList(mainButtonView.toggleGroup, mainButtonView.toggleList);
		mainButtonModel.toggleSubject.Subscribe(tgl =>
		{
			mainButtonView.DeactivateAllPanel(bottomPanelView.panelList);
			mainButtonView.ActivatePanel(tgl.type);
		}).AddTo(this);
	}

	public void CurrencySubscribe()
	{
		CurrencyManager<Gold>.GetCurrency(ECurrencyType.GOLD).Subscribe(gold =>
		{
			currencyView.gold_txt.text = gold.ToAlphabetNumber();
		}).AddTo(currencyView.gameObject);

		CurrencyManager<Dia>.GetCurrency(ECurrencyType.DIA).Subscribe(dia =>
		{
			currencyView.dia_txt.text = dia.ToAlphabetNumber();
		}).AddTo(currencyView.gameObject);

		CurrencyManager<Key>.GetCurrency(ECurrencyType.KEY).Subscribe(key =>
		{
			currencyView.key_txt.text = key.ToAlphabetNumber();
		}).AddTo(currencyView.gameObject);
	}
}

