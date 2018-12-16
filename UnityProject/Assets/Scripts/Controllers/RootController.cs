using System.Collections.Generic;
using System.Linq;
using Controllers.Core;
using TestProject.Model;

namespace TestProject.Controllers
{
    public class RootController : ControllerBase
    {
        private readonly BattleData _battleData;
        private IAiInput _aiInput;

        public RootController(BattleData battleData, IAiInput aiInput, ControllerFactory controllerFactory) : base(
            controllerFactory)
        {
            _battleData = battleData;
            _aiInput = aiInput;
        }

        protected override void OnStart()
        {
            if (_aiInput.IsTrainRun)
            {
                PlayCycle();
            }
            else
            {
                FillBattleData();
                _aiInput.Initialize(_battleData);
                CreateAndStart<BattleFlowController>(true);
            }
        }

        private async void PlayCycle()
        {
            while (IsControllerAlive)
            {
                FillBattleData();
                _aiInput.Initialize(_battleData);
                var res = await CreateAndStart<BattleFlowController>(false).GetProcessedTask();
                RemoveController(res.Controller);
                _aiInput.Done(GetWinner());
            }
        }

        private ArmyType GetWinner()
        {
            var isPlayerWin = _battleData.Soldiers.Any(item => item.IsAlive && item.ArmyType == ArmyType.Player);
            return isPlayerWin ? ArmyType.Player : ArmyType.Opponent;
        }

        private void FillBattleData()
        {
            _battleData.Soldiers = CreateSoldiers(8);
            _battleData.GridWidth = 10;
            _battleData.GridHeight = 8;
            _battleData.AttackDistance = 5;
        }

        private SoldierData[] CreateSoldiers(int armySize)
        {
            var soldiers = new List<SoldierData>();
            for (int i = 1; i <= armySize; i++)
            {
                var playerSoldier = new SoldierData
                    {ArmyType = ArmyType.Player, Id = i, IsAlive = true, PositionX = 1, PositionY = i};
                var opponentSoldier = new SoldierData
                    {ArmyType = ArmyType.Opponent, Id = armySize + i, IsAlive = true, PositionX = 10, PositionY = i};
              
                soldiers.Add(playerSoldier);
                soldiers.Add(opponentSoldier);
            }

            return soldiers.ToArray();
        }
    }
}