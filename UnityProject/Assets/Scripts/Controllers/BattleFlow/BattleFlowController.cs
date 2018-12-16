using System;
using System.Linq;
using System.Threading.Tasks;
using Controllers.Core;
using TestProject.Model;
using UnityEngine;
using UnityEngine.Assertions;

namespace TestProject.Controllers
{
    public class BattleFlowController : ControllerWithResultBase<BattleFlowControllerResult>
    {
        private readonly IBattleView _battleView;
        private readonly BattleData _battleData;
        private readonly IBattleUI _battleUi;

        private int _currentSoldierIndex;
        private bool _isPvE;
        private ArmyType _winner;

        private bool IsBattleInProgress => IsArmySoldierAlive(ArmyType.Player) && IsArmySoldierAlive(ArmyType.Opponent);

        public BattleFlowController(IBattleView battleView, IBattleUI battleUi, BattleData battleData,
            ControllerFactory controllerFactory)
            : base(controllerFactory)
        {
            _battleView = battleView;
            _battleData = battleData;
            _battleUi = battleUi;
        }

        protected override void SetArg(object arg)
        {
            Assert.IsTrue(arg is bool);
            _isPvE = (bool) arg;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _battleView.Initialize(_battleData);
            _battleUi.Activate(_isPvE);

            RunBattleCycle();
        }

        protected override void OnStop()
        {
            _battleView.Hide();
            _battleUi.Deactivate();
        }

        protected override void AddEventHandlers()
        {
            _battleUi.ContinuePressed += BattleUi_ContinuePressed;
            _battleUi.LeavePressed += BattleUi_LeavePressed;
        }

        protected override void RemoveEventHandlers()
        {
            _battleUi.ContinuePressed -= BattleUi_ContinuePressed;
            _battleUi.LeavePressed -= BattleUi_LeavePressed;
        }

        private async void RunBattleCycle()
        {
            try
            {
                while (IsBattleInProgress && IsControllerAlive)
                {
                    var soldier = GetCurrentSoldier();
                    var inputType = GetInputType(soldier.ArmyType);
                    var arg = new Tuple<SoldierData, InputType>(soldier, inputType);

                    var moveRes = await CreateAndStart<MoveController>(arg).GetProcessedTask();
                    RemoveController(moveRes.Controller);
                    _battleUi.AddLog(
                        $"({soldier.ArmyType})Soldier_{soldier.Id} Moved to {soldier.PositionX} {soldier.PositionY}");

                    if (IsControllerAlive)
                    {
                        var attackRes = await CreateAndStart<AttackController>(arg).GetProcessedTask();
                        RemoveController(attackRes.Controller);

                        if (attackRes.AttackedSoldierId.HasValue)
                        {
                            _battleUi.AddLog($"({soldier.ArmyType})Soldier_{soldier.Id} Attacked soldier {attackRes.AttackedSoldierId.Value}.");
                        }
                        else
                        {
                            _battleUi.AddLog($"({soldier.ArmyType})Soldier_{soldier.Id} has no aims for attack.");
                        }
                    }
                }

                _winner = GetWinner();
                var finishMessage = $"Battle finished! Winner is {_winner}";
                _battleUi.AddLog(finishMessage);
                if (_isPvE)
                {
                    _battleUi.ShowResultMessage(finishMessage);
                }
                else
                {
                    Complete(new BattleFlowControllerResult(_winner, this));
                }
            }
            catch (TaskCanceledException)
            {
                Debug.LogWarning("Controller stopped while await.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error during battle cycle. Ex: {e}");
                Complete(new BattleFlowControllerResult(ArmyType.Opponent, this));
            }
        }

        private InputType GetInputType(ArmyType soldierArmyType)
        {
            if (_isPvE)
            {
                return soldierArmyType == ArmyType.Player ? InputType.Player : InputType.AI;
            }

            return InputType.AI;
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

        private ArmyType GetWinner()
        {
            var isPlayerWin = _battleData.Soldiers.Any(item => item.IsAlive && item.ArmyType == ArmyType.Player);
            return isPlayerWin ? ArmyType.Player : ArmyType.Opponent;
        }

        private bool IsArmySoldierAlive(ArmyType armyType)
        {
            return _battleData.Soldiers.Any(item => item.ArmyType == armyType && item.IsAlive);
        }

        private void BattleUi_LeavePressed()
        {
            Complete(new BattleFlowControllerResult(ArmyType.Opponent, this));
        }

        private void BattleUi_ContinuePressed()
        {
            Complete(new BattleFlowControllerResult(_winner, this));
        }
    }
}