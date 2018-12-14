using System.Collections.Generic;
using System.Linq;
using TestProject.Controllers;
using TestProject.Model;
using UnityEngine;

namespace TestProject.Views
{
    public class BattleView : MonoBehaviour, IBattleView
    {
        private readonly List<TileView> _tileViews = new List<TileView>();
        private readonly List<SoldierView> _soldierViews = new List<SoldierView>();

        [SerializeField] private TileView _tilePrefab;
        [SerializeField] private SoldierView _soldierPrefab;
        [SerializeField] private Transform _firstTileAnchor;

        public void Initialize(BattleData battleData)
        {
            CreateGrid(battleData.GridWidth, battleData.GridHeight);
            CreateSoldiers(battleData.Soldiers);
        }

        public void Move(int id, int positionX, int positionY)
        {
            var soldier = GetSoldierView(id);
            soldier.MoveToPosition(GetWorldPosition(positionX, positionY));
        }

        public void Kill(int id)
        {
            var soldier = GetSoldierView(id);
            soldier.Kill();
        }  

        private void CreateGrid(int height, int width)
        {
            ClearGrid();

            for (int i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var position = _firstTileAnchor.position + new Vector3(-j, 0, i);
                    var tileView = Instantiate(_tilePrefab, position, _firstTileAnchor.rotation, transform);
                    tileView.Initialize(i + 1, j + 1);
                    _tileViews.Add(tileView);
                }
            }
        }

        private void CreateSoldiers(SoldierData[] soldiers)
        {
            ClearSoldiers();

            foreach (var soldier in soldiers)
            {
                var position = GetWorldPosition(soldier.PositionX, soldier.PositionY);
                var soldierView = Instantiate(_soldierPrefab, transform);
                soldierView.Initialize(soldier.Id, soldier.ArmyType, position);
                _soldierViews.Add(soldierView);
            }
        }

        private Vector3 GetWorldPosition(int soldierPositionX, int soldierPositionY)
        {
            var tileView = _tileViews.First(item => item.X == soldierPositionX && item.Y == soldierPositionY);
            return tileView.Position;
        }

        private SoldierView GetSoldierView(int id)
        {
            return _soldierViews.First(item => item.Id == id);
        }
        
        private void ClearGrid()
        {
            foreach (var tile in _tileViews)
            {
                Destroy(tile.gameObject);
            }

            _tileViews.Clear();
        }

        private void ClearSoldiers()
        {
            foreach (var soldier in _soldierViews)
            {
                Destroy(soldier.gameObject);
            }

            _soldierViews.Clear();
        }
    }
}