using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Controllers.Core
{
    [RequireComponent(typeof(SceneContext))]
    public class SceneContextProvider : MonoBehaviour
    {
        public static DiContainer Container { get; private set; }

        private void Start()
        {
            DontDestroyOnLoad(this);
            Container = GetComponent<SceneContext>().Container;
        }
    }
}