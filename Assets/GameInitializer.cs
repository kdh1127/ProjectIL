using UnityEngine;
using Zenject;
public class GameInitializer : MonoBehaviour
{
	[Inject] public QuestPresenter questPresenter;

	private void Start()
	{
		questPresenter.Subscribe();
	}
}
