using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		Container.Bind<CharacterView>().FromComponentInHierarchy().AsTransient();
	}
}
