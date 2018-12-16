using Controllers.Core;

namespace TestProject.Controllers
{
    public class AttackControllerResult : ControllerResultBase
    {
        public int? AttackedSoldierId { get; }
        public AttackControllerResult(ControllerBase controller) : base(controller)
        {
        }
        
        public AttackControllerResult(int attackedSoldierId, ControllerBase controller) : base(controller)
        {
            AttackedSoldierId = attackedSoldierId;
        }
    }
}