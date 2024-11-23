using UnityEngine;
using Zenject;
public class TestManager : MonoBehaviour
{
	[Inject]private readonly CurrencyModel.Gold gold;
	[Inject] private readonly CurrencyModel.Key key;


	public void ShowMeTheGold()
	{
		gold.Add(1000000000);
	}

	public void ShowMeTheKey()
	{
		key.Add(100000000000);
	}
}
