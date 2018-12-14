using System;
using System.Linq;
using Controllers.Core;
using TestProject.Model;
using UnityEngine.Assertions;

namespace TestProject.Controllers
{
    public class AttackController : ControllerWithResultBase<ControllerResultBase>
    {
        private readonly IAttackInput _attackInput;
        private readonly BattleData _battleData;
        private readonly IBattleView _battleView;
        private SoldierData _currentSoldier;

        public AttackController(IAttackInput attackInput, IBattleView battleView, BattleData battleData, ControllerFactory controllerFactory) :
            base(controllerFactory)
        {
            _attackInput = attackInput;
            _battleView = battleView;
            _battleData = battleData;
        }

        protected override void SetArg(object arg)
        {
            Assert.IsTrue(arg is SoldierData);
            _currentSoldier = (SoldierData) arg;
        }

        protected override void OnStart()
        {
            base.OnStart();
            var availableTargets = GetAvailableTargets();

            _attackInput.Activate(availableTargets);

            if (availableTargets.Length == 0)
            {
                Complete(new ControllerResultBase(this));
            }
        }

        protected override void OnStop()
        {
            _attackInput.Deactivate();
        }

        protected override void AddEventHandlers()
        {
            _attackInput.TargetSelected += AttackInput_TargetSelected;
        }

        protected override void RemoveEventHandlers()
        {
            _attackInput.TargetSelected -= AttackInput_TargetSelected;
        }

        private int[] GetAvailableTargets()
        {
            var soldiers = _battleData.Soldiers.Where(item =>
                item.ArmyType != _currentSoldier.ArmyType && item.IsAlive && IsSoldierReachable(item));
            var ids = soldiers.Select(item => item.Id).ToArray();
            return ids;
        }

        private bool IsSoldierReachable(SoldierData soldier)
        {
            var distanceH = Math.Abs(_currentSoldier.PositionX - soldier.PositionX);
            var distanceV = Math.Abs(_currentSoldier.PositionY - soldier.PositionY);
            var maxDistance = Math.Max(distanceH, distanceV);
            return distanceH * distanceV == 0 && maxDistance < _battleData.AttackDistance;
        }
        
        private void ProcessTargetSelect(int targetId)
        {
            _battleView.Kill(targetId);
            var soldier = _battleData.Soldiers.First(item => item.Id == targetId);
            soldier.IsAlive = false;
            Complete(new ControllerResultBase(this));
        }
        
        private void AttackInput_TargetSelected(int targetId)
        {
            ProcessTargetSelect(targetId);
        }
    }
}