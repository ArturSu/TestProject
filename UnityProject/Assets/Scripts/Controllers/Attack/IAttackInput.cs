using System;

namespace TestProject.Controllers
{
    public interface IAttackInput
    {
        event Action<int> TargetSelected; 
        InputType InputType { get; }
        void Activate(int[] targetIds);
        void Deactivate();
    }
}