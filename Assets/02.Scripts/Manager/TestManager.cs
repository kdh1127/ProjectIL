using UnityEngine;
using Zenject;
public class TestManager : MonoBehaviour
{
	[Inject]private readonly CurrencyModel.Gold gold;
	[Inject] private readonly CurrencyModel.Key key;


	public void ShowMeTheMoney()
	{
		//gold.Add(100000);
		key.Add(100000000000);
	}
}
