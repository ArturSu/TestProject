using Controllers.Core;
using TestProject.Controllers;
using TestProject.Controllers.SetupArmies;
using TestProject.Model;
using Zenject;

namespace TestProject
{
    public class ControllersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<RootController>().AsTransient();
            Container.Bind<BattleFlowController>().AsTransient();
            Container.Bind<SetupArmiesController>().AsTransient();
            Container.Bind<AttackController>().AsTransient();
            Container.Bind<MoveController>().AsTransient();
            Container.Bind<ControllerFactory>().AsSingle();
            Container.Bind<BattleData>().AsSingle();
        }
    }
}