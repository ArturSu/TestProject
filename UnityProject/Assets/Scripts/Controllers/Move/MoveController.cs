using System;
using System.Collections.Generic;
using System.Linq;
using Controllers.Core;
using TestProject.Model;
using UnityEngine.Assertions;
using Coordinates = System.Tuple<int, int>;

namespace TestProject.Controllers
{
    public class MoveController : ControllerWithResultBase
    {
        private readonly BattleData _battleData;
        private readonly IMoveInput _moveInput;
        private readonly IBattleView _battleView;
        private SoldierData _currentSoldier;

        public MoveController(IMoveInput moveInput, IBattleView battleView, BattleData battleData,
            ControllerFactory controllerFactory) : base(controllerFactory)
        {
            _battleData = battleData;
            _moveInput = moveInput;
            _battleView = battleView;
        }

        protected override void SetArg(object arg)
        {
            Assert.IsTrue(arg is SoldierData);
            _currentSoldier = (SoldierData) arg;
        }

        protected override void OnStart()
        {
            base.OnStart();
            var availableDirections = GetAvailableDirections();

            _moveInput.Activate(availableDirections);

            if (availableDirections.Length == 0)
            {
                Complete(new ControllerResultBase(this));
            }
        }

        protected override void OnStop()
        {
            _moveInput.Deactivate();
        }

        protected override void AddEventHandlers()
        {
            _moveInput.TileSelected += MoveInput_TileSelected;
        }

        protected override void RemoveEventHandlers()
        {
            _moveInput.TileSelected -= MoveInput_TileSelected;
        }

        private Coordinates[] GetAvailableDirections()
        {
            var list = new List<Coordinates>();

            var directions = Enum.GetValues(typeof(MoveDirection));
            
            foreach (MoveDirection direction in directions)
            {
                var delta = GetDeltaByDirection(direction);
                var positionX = _currentSoldier.PositionX + delta.Item1;
                var positionY = _currentSoldier.PositionY + delta.Item2;
                if (IsEmptyAndValidPosition(positionX, positionY))
                {
                    list.Add(new Coordinates(positionX, positionY));
                }
            }

            return list.ToArray();
        }

        private bool IsEmptyAndValidPosition(int positionX, int positionY)
        {
            if (positionX > 0 && positionX <= _battleData.GridWidth)
            {
                if (positionY > 0 && positionY <= _battleData.GridHeight)
                {
                    return !_battleData.Soldiers.Any(item =>
                        item.PositionX == positionX && item.PositionY == positionY && item.IsAlive);
                }
            }

            return false;
        }

        private void ProcessMove(Coordinates tile)
        {            
            _currentSoldier.PositionX = tile.Item1;
            _currentSoldier.PositionY = tile.Item2;

            _battleView.Move(_currentSoldier.Id, _currentSoldier.PositionX, _currentSoldier.PositionY);

            Complete(new ControllerResultBase(this));
        }

        private Tuple<int, int> GetDeltaByDirection(MoveDirection moveDirection)
        {
            Tuple<int, int> delta;
            switch (moveDirection)
            {
                case MoveDirection.Up:
                    delta  = new Tuple<int, int>(0, 1);
                    break;
                case MoveDirection.Down:
                    delta  = new Tuple<int, int>(0, -1);
                    break;
                case MoveDirection.Right:
                    delta  = new Tuple<int, int>(1, 0);
                    break;
                case MoveDirection.Left:
                    delta  = new Tuple<int, int>(-1, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(moveDirection), moveDirection, null);
            }

            return delta;
        }

        private void MoveInput_TileSelected(Coordinates tile)
        {
            ProcessMove(tile);
        }
    }
}