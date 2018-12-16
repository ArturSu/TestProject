using UnityEngine;
using Zenject;

namespace Controllers.Core
{
    public class ControllerFactory
    {
        private static DiContainer Container => SceneContextProvider.Container;

        public T Create<T>(object arg = null) where T : ControllerBase
        {
            try
            {
                var controller = Container.Resolve<T>();
                controller.Initialize(arg);
                return controller;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Can't create controller. Exc: {ex}");
            }

            return default(T);
        }

        public T Create<T>(System.Type type, object arg = null) where T : ControllerBase
        {
            try
            {
                var controller = (T)Container.Resolve(type);
                controller.Initialize(arg);
                return controller;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Can't create controller. Exc: {ex}");
            }

            return default(T);
        }
    }
}