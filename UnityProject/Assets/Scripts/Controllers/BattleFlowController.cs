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
        
        private bool IsBattleInProgress => IsControllerAlive && _battleData.Soldiers.Any(item => item.IsAlive);

        public BattleFlowController(IBattleView battleView, BattleData battleData, ControllerFactory controllerFactory)
            : base(controllerFactory)
        {
            _battleView = battleView;
            _battleData = battleData;
        }

        protected override void OnStart()
        {
            base.OnStart();
            var playerSoldiers = _battleData.Soldiers.Where(item => item.ArmyType == ArmyType.Player)
                .Select(item => item.Id).ToArray();
            var opponentSoldiers = _battleData.Soldiers.Where(item => item.ArmyType == ArmyType.Opponent)
                .Select(item => item.Id).ToArray();

            _battleView.Initialize(playerSoldiers, opponentSoldiers);

            RunBattleCycle();
        }

        private async void RunBattleCycle()
        {
            try
            {
                while (IsBattleInProgress)
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
            for (var i = _currentSoldierIndex; i < _battleData.Soldiers.Length; i++)
            {
                var soldier = _battleData.Soldiers[i];
                if (soldier.IsAlive)
                {
                    _currentSoldierIndex = (i + 1) % _battleData.Soldiers.Length;
                    return soldier;
                }
            }

            throw new Exception("Can't find alive soldier in array.");
        }
    }
}