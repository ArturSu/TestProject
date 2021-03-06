using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TestProject.Views
{
    public class TileView : MonoBehaviour, IPointerClickHandler
    {
        public event Action<int, int> Clicked; 
        
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Collider _collider;
        
        public int Y { get; private set; }
        public int X { get; private set; }
        public Vector3 Position => transform.position;
        
        public void Initialize(int x, int y)
        {
            X = x;
            Y = y;
            gameObject.name = $"Tile_{x}_{y}";
            SetDefaultState();
        }

        public void SetDefaultState()
        {
            _meshRenderer.material.color = Color.white;
            _collider.enabled = false;
        }
        
        public void SetMoveState()
        {
            _meshRenderer.material.color = Color.green;
            _collider.enabled = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnTileClicked(X, Y);
        }

        private void OnTileClicked(int x, int y)
        {
            Clicked?.Invoke(x, y);
        }
    }
}