using System;
using TestProject.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TestProject.Views
{
    public class SoldierView : MonoBehaviour, IPointerClickHandler
    {
        public event Action<int> Clicked;
        
        public int Id { get; private set; }

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private GameObject _attackHighlight;
        [SerializeField] private Collider _collider;
        
        public void Initialize(int id, ArmyType armyType, Vector3 position)
        {
            Id = id;
           _meshRenderer.material.color = armyType == ArmyType.Player ? Color.blue : Color.black;
            transform.position = position;
            gameObject.SetActive(true);
            SetDefaultState();
        }

        public void MoveToPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void Kill()
        {
            gameObject.SetActive(false);
        }
        
        public void SetAttackState()
        {
            _attackHighlight.SetActive(true);
            _collider.enabled = true;
        }

        public void SetDefaultState()
        {
            _attackHighlight.SetActive(false);
            _collider.enabled = false;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnSoldierClicked(Id);
        }
        
        private void OnSoldierClicked(int id)
        {
            Clicked?.Invoke(id);
        }
    }
}