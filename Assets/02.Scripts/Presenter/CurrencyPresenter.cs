using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Zenject;
using UniRx;
public class CurrencyPresenter
{
	private readonly CurrencyModel model;
	private readonly CurrencyView view;

	[Inject]
	public CurrencyPresenter (CurrencyModel model, CurrencyView view)
	{
		this.model = model;
		this.view = view;
	}

	public void Subscribe()
	{
		model.gold.Subscribe(gold =>
		{
			model.Save(ECurrencyType.GOLD);
			view.gold_txt.text = gold.ToAlphabetNumber();
		}).AddTo(view.gameObject);

		model.dia.Subscribe(dia =>
		{
			model.Save(ECurrencyType.DIA);
			view.dia_txt.text = dia.ToAlphabetNumber();
		}).AddTo(view.gameObject);

		model.key.Subscribe(key =>
		{
			model.Save(ECurrencyType.KEY);
			view.key_txt.text = key.ToAlphabetNumber();
		}).AddTo(view.gameObject);
	}

}
