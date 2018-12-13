using Controllers.Core;

namespace TestProject.Controllers
{
    public class AttackResult : ControllerResultBase
    {
        public int? TargetId { get; }
        
        public AttackResult(int? targetId, ControllerBase controller) : base(controller)
        {
            TargetId = targetId;
        }
    }
}