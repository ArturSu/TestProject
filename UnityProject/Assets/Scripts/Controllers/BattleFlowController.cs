using System;
using System.Linq;
using System.Threading.Tasks;
using Controllers.Core;
using TestProject.Model;
using UnityEngine;

namespace TestProject.Controllers
{
    public class BattleFlowController : ControllerWithResultBase<ControllerResultBase>
    {
        private readonly IBattleView _battleView;
        private readonly BattleData _battleData;

        private int _currentSoldierIndex;

        private bool IsBattleInProgress => IsArmySoldierAlive(ArmyType.Player) && IsArmySoldierAlive(ArmyType.Opponent);

        public BattleFlowController(IBattleView battleView, BattleData battleData, ControllerFactory controllerFactory)
            : base(controllerFactory)
        {
            _battleView = battleView;
            _battleData = battleData;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _battleView.Initialize(_battleData);

            RunBattleCycle();
        }

        private async void RunBattleCycle()
        {
            try
            {
                while (IsBattleInProgress && IsControllerAlive)
                {
                    var soldier = GetCurrentSoldier();

                    var moveRes = await CreateAndStart<MoveController>(soldier).GetProcessedTask();
                    RemoveController(moveRes.Controller);

                    if (IsControllerAlive)
                    {
                        var attackRes = await CreateAndStart<AttackController>(soldier).GetProcessedTask();
                        RemoveController(attackRes.Controller);
                    }
                }
            }
            catch (TaskCanceledException)
            {
                Debug.LogWarning("Controller stopped while await.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error during battle cycle. Ex: {e}");
                Complete(new ControllerResultBase(this));
            }
        }

        private SoldierData GetCurrentSoldier()
        {
            var firstCheck = true;
            var startIndex = _currentSoldierIndex;
            while (startIndex != _currentSoldierIndex || firstCheck)
            {
                firstCheck = false;
                var soldier = _battleData.Soldiers[_currentSoldierIndex];
               
                _currentSoldierIndex++;
                _currentSoldierIndex %= _battleData.Soldiers.Length;
                
                if (soldier.IsAlive)
                {
                    return soldier;
                }
            }

            throw new Exception("Can't find alive soldier in array.");
        }

        private bool IsArmySoldierAlive(ArmyType armyType)
        {
            return _battleData.Soldiers.Any(item => item.ArmyType == armyType && item.IsAlive);
        }
    }
}