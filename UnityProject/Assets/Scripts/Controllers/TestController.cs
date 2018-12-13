using Controllers.Core;
using UnityEngine;

namespace TestProject.Controllers
{
    public class TestController : ControllerWithResultBase
    {
        public TestController(ControllerFactory controllerFactory) : base(controllerFactory)
        {
        }

        protected override void OnStart()
        {
            base.OnStart();
            TestAsync();
        }

        private async void TestAsync()
        {
            for (int i = 0; i < 3; i++)
            {
                var res = await CreateAndStart<TestControllerB>().GetProcessedTask();
                RemoveController(res.Controller);
            }

            Debug.Log("TestController. Finish test.");
        }
    }
}