using UnityEngine;
using Zenject;
public class TestManager : MonoBehaviour
{
	[Inject] private readonly CurrencyModel.Gold gold;
	[Inject] private readonly CurrencyModel.Key key;
	[Inject] private readonly CurrencyModel.Dia dia;


	public void ShowMeTheGold()
	{
		gold.Add(1000000000);
	}

	public void ShowMeTheKey()
	{
		key.Add(100000000000);
	}
	public void ShowMeTheDia()
	{
		dia.Add(100);
	}
}
