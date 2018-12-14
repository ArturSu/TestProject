using TestProject.Model;
using UnityEngine;

namespace TestProject.Views
{
    public class SoldierView : MonoBehaviour
    {
        public int Id { get; private set; }

        [SerializeField] 
        private MeshRenderer _meshRenderer;
        
        public void Initialize(int id, ArmyType armyType, Vector3 position)
        {
            Id = id;
           _meshRenderer.material.color = armyType == ArmyType.Player ? Color.blue : Color.black;
            transform.position = position;
            gameObject.SetActive(true);
        }

        public void MoveToPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void Kill()
        {
            gameObject.SetActive(false);
        }
    }
}