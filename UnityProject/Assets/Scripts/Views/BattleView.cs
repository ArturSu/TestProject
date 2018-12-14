using System;
using System.Collections.Generic;
using System.Linq;
using TestProject.Controllers;
using TestProject.Model;
using UnityEngine;

namespace TestProject.Views
{
    public class BattleView : MonoBehaviour, IBattleView, IMoveInput, IAttackInput
    {
        private readonly List<TileView> _tileViews = new List<TileView>();
        private readonly List<SoldierView> _soldierViews = new List<SoldierView>();

        public event Action<Tuple<int, int>> TileSelected;
        public event Action<int> TargetSelected;

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

        public void Activate(Tuple<int, int>[] tiles)
        {
            foreach (var tile in tiles)
            {
                var tileView = _tileViews.First(item => item.X == tile.Item1 && item.Y == tile.Item2);
                tileView.SetMoveState();
            }
        }

        public void Activate(int[] targetIds)
        {
            foreach (var targetId in targetIds)
            {
                var soldierView = _soldierViews.First(item => item.Id == targetId);
                soldierView.SetAttackState();
            }
        }

        void IAttackInput.Deactivate()
        {
            foreach (var soldierView in _soldierViews)
            {
                soldierView.SetDefaultState();
            }
        }

        void IMoveInput.Deactivate()
        {
            foreach (var tileView in _tileViews)
            {
                tileView.SetDefaultState();
            }
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
                    tileView.TileClicked += TileView_TileClicked;
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
                soldierView.SoldierClicked +=SoldierView_SoldierClicked;
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
            foreach (var tileView in _tileViews)
            {
                tileView.TileClicked -= TileView_TileClicked;
                Destroy(tileView.gameObject);
            }

            _tileViews.Clear();
        }

        private void ClearSoldiers()
        {
            foreach (var soldierView in _soldierViews)
            {
                soldierView.SoldierClicked -=SoldierView_SoldierClicked;
                Destroy(soldierView.gameObject);
            }

            _soldierViews.Clear();
        }
        
        private void OnTileSelected(Tuple<int, int> tile)
        {
            TileSelected?.Invoke(tile);
        }
        
        private void OnTargetSelected(int id)
        {
            TargetSelected?.Invoke(id);
        }

        private void TileView_TileClicked(int x, int y)
        {
            OnTileSelected(new Tuple<int, int>(x, y));
        }
        
        private void SoldierView_SoldierClicked(int id)
        {
            OnTargetSelected(id);
        }
    }
}