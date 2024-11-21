using UnityEngine;
using Zenject;
public class TestManager : MonoBehaviour
{
	[Inject]private readonly CurrencyModel.Gold gold;


	public void ShowMeTheMoney()
	{
		gold.Add(100000);
	}
}
