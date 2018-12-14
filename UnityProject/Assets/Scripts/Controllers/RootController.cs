using Controllers.Core;
using TestProject.Model;

namespace TestProject.Controllers
{
    public class RootController : ControllerBase
    {
        private readonly BattleData _battleData;

        public RootController(BattleData battleData, ControllerFactory controllerFactory) : base(controllerFactory)
        {
            _battleData = battleData;
        }

        protected override void OnStart()
        {
            FillBattleData();
        //    CreateAndStart<BattleFlowController>();
        }

        private void FillBattleData()
        {
            var playerSoldier = new SoldierData
                {ArmyType = ArmyType.Player, Id = 1, IsAlive = true, PositionX = 1, PositionY = 2};
            var opponentSoldier = new SoldierData
                {ArmyType = ArmyType.Opponent, Id = 2, IsAlive = true, PositionX = 5, PositionY = 2};

            _battleData.Soldiers = new[] {playerSoldier, opponentSoldier};
            _battleData.GridWidth = 10;
            _battleData.GridHeight = 8;
            _battleData.AttackDistance = 5;
        }
    }
}