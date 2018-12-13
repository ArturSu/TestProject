using Controllers.Core;
using UnityEngine;
using Zenject;

namespace TestProject.Controllers
{
    public class GameEntryPoint : MonoBehaviour
    {
        [Inject] 
        private ControllerFactory _controllerFactory;

        private bool _isGameInitialized;
        
        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            if (!_isGameInitialized)
            {
                var rootController = _controllerFactory.Create<TestController>();
                rootController.Start();
                _isGameInitialized = true;
            }
        }
    }
}