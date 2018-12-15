using Controllers.Core;
using TestProject.Model;

namespace TestProject.Controllers
{
    public class RootController : ControllerBase
    {
        private readonly BattleData _battleData;
        private IAiInput _aiInput;

        public RootController(BattleData battleData, IAiInput aiInput, ControllerFactory controllerFactory) : base(controllerFactory)
        {
            _battleData = battleData;
            _aiInput = aiInput;
        }

        protected override void OnStart()
        {
            if (_aiInput.IsTrainRun)
            {
                PlayCicle();
            }
            else
            {
                FillBattleData();
                _aiInput.Initialize(_battleData);
                CreateAndStart<BattleFlowController>(true);
                _aiInput.Done();
            }
        }

        private async void PlayCicle()
        {
            while (IsControllerAlive)
            {
                FillBattleData();
                _aiInput.Initialize(_battleData);
                var res = await CreateAndStart<BattleFlowController>(false).GetProcessedTask();
                RemoveController(res.Controller);
                _aiInput.Done();
            }
        }

        private void FillBattleData()
        {
            var playerSoldier1 = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 1, PositionY = 2};
            var playerSoldier2 = new SoldierData
                {ArmyType = ArmyType.Player, Id = 3, IsAlive = true, PositionX = 1, PositionY = 4};
            var opponentSoldier1 = new SoldierData
                {ArmyType = ArmyType.Opponent, Id = 2, IsAlive = true, PositionX = 7, PositionY = 2};
            var opponentSoldier2 = new SoldierData
                {ArmyType = ArmyType.Opponent, Id = 4, IsAlive = true, PositionX = 7, PositionY = 4};

            _battleData.Soldiers = new[] {playerSoldier1, opponentSoldier1};
            _battleData.GridWidth = 10;
            _battleData.GridHeight = 8;
            _battleData.AttackDistance = 5;
        }
    }
}