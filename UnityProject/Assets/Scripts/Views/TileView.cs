using UnityEngine;

namespace TestProject.Views
{
    public class TileView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        public int Y { get; private set; }
        public int X { get; private set; }
        public Vector3 Position => transform.position;
        
        public void Initialize(int x, int y)
        {
            _meshRenderer.material.color = Color.white;
            X = x;
            Y = y;
        }

        public void SetMoveState()
        {
            _meshRenderer.material.color = Color.green;
        }
        
        public void SetAttackState()
        {
            _meshRenderer.material.color = Color.red;
        }
    }
}