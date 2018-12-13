using System.Threading.Tasks;
using Controllers.Core;
using UnityEngine;

namespace TestProject.Controllers
{
    public class TestControllerB : ControllerWithResultBase
    {
        public TestControllerB(ControllerFactory controllerFactory) : base(controllerFactory)
        {
        }

        protected override void OnStart()
        {
            base.OnStart();
            TestAsync();
        }

        private async void TestAsync()
        {
            Debug.Log("Start TestAsync");
            await Task.Delay(1000);
            Debug.Log("Complete TestAsync");
            Complete(new ControllerResultBase(this));
        }
    }
}