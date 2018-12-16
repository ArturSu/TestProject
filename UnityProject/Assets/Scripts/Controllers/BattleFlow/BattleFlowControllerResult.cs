using Controllers.Core;
using TestProject.Model;

namespace TestProject.Controllers
{
    public class BattleFlowControllerResult : ControllerResultBase
    {
        public ArmyType Winner { get; }
        
        public BattleFlowControllerResult(ArmyType winner, ControllerBase controller) : base(controller)
        {
            Winner = winner;
        }
    }
}