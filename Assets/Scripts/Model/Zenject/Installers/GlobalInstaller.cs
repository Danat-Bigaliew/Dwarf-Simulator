using Zenject;

public class GlobalInstaller : MonoInstaller
{
    public PlayerDataOnSession playerDataOnSession;
    public POSTRequest postRequest;
    public ClicksController clicksController;

    public override void InstallBindings()
    {
        Container.Bind<PlayerDataOnSession>().FromComponentInHierarchy().AsSingle();
        Container.Bind<POSTRequest>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ClicksController>().FromComponentInHierarchy().AsSingle();

        Container.Bind<PlayerDataService>().FromComponentInHierarchy(playerDataOnSession).AsSingle();
        Container.Bind<PostRequest>().FromComponentInHierarchy(postRequest).AsSingle();
    }
}