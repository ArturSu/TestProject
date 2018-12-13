using Controllers.Core;
using TestProject.Controllers;
using Zenject;

namespace TestProject
{
    public class ControllersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<TestController>().AsTransient();
            Container.Bind<TestControllerB>().AsTransient();
            Container.Bind<ControllerFactory>().AsSingle();
        }
    }
}