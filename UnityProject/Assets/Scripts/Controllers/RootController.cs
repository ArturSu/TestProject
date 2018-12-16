using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controllers.Core;
using TestProject.Controllers.SetupArmies;
using TestProject.Model;

namespace TestProject.Controllers
{
    public class RootController : ControllerBase
    {
        private readonly BattleData _battleData;
        private readonly IAiInput _aiInput;

        public RootController(BattleData battleData, IAiInput aiInput, ControllerFactory controllerFactory) : base(
            controllerFactory)
        {
            _battleData = battleData;
            _aiInput = aiInput;
        }

        protected override void OnStart()
        {
            PlayCycle(_aiInput.IsTrainRun);
        }

        private async void PlayCycle(bool isTrainRun)
        {
            while (IsControllerAlive)
            {
                var res = await CreateAndStart<SetupArmiesController>(!isTrainRun).GetProcessedTask();
                RemoveController(res.Controller);
                _aiInput.Initialize(_battleData);
                res = await CreateAndStart<BattleFlowController>(!isTrainRun).GetProcessedTask();
                RemoveController(res.Controller);
                _aiInput.Done(GetWinner());
            }
        }

        private ArmyType GetWinner()
        {
            var isPlayerWin = _battleData.Soldiers.Any(item => item.IsAlive && item.ArmyType == ArmyType.Player);
            return isPlayerWin ? ArmyType.Player : ArmyType.Opponent;
        }
    }
}