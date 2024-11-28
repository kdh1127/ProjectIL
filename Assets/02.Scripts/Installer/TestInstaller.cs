using Zenject;

public class TestInstaller : MonoInstaller
{
	public TestManager testComponent;
	public override void InstallBindings()
	{
		Container.Bind<TestManager>().FromInstance(testComponent).AsSingle();
	}
}
