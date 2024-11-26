using UnityEngine;
using Zenject;
public class TestManager : MonoBehaviour
{
	[Inject] private readonly CurrencyModel.Gold gold;
	[Inject] private readonly CurrencyModel.Key key;
	[Inject] private readonly CurrencyModel.Dia dia;


	public void ShowMeTheGold()
	{
		gold.Add(1000);
	}

	public void ShowMeTheKey()
	{
		key.Add(1000);
	}
	public void ShowMeTheDia()
	{
		dia.Add(1000);
	}
}
