using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class ResourcesInstaller : MonoInstaller
{
	public TRScriptableManager trScriptableManager;

	public override void InstallBindings()
	{
		Container.Bind<Dictionary<string, Sprite>>()
			.WithId("CostImage")
			.FromInstance(trScriptableManager.GetSprite("CostImageResources").spriteDictionary)
			.AsCached();

		Container.Bind<Dictionary<string, Sprite>>()
			.WithId("QuestImage")
			.FromInstance(trScriptableManager.GetSprite("QuestImageResources").spriteDictionary)
			.AsCached();
	}

}
