using System;
using UnityEngine;
using Zenject;
public class GameInitializer : MonoBehaviour
{
	[Inject] public QuestPresenter questPresenter;
	[Inject] public QuestModel questModel;

	[Inject] public WeaponPresenter weaponPresenter;
	[Inject] public WeaponModel weaponModel;

    private void Awake()
	{
		InitTable();
		InitModel();
	}

	private void Start()
	{
		SubscirbePresnters();
	}

	private void InitModel()
    {
		questModel.Init(QuestTableList.Get());
		weaponModel.Init(WeaponTableList.Get());
    }

	private void InitTable()
	{
		QuestTableList.Init(TRScriptableManager.Instance.GetGoogleSheet("QuestTable"));
		WeaponTableList.Init(TRScriptableManager.Instance.GetGoogleSheet("WeaponTable"));

	}

	private void SubscirbePresnters()
    {
		questPresenter.Subscribe();
		weaponPresenter.WeaponPanelSubscribe();
	}
}
