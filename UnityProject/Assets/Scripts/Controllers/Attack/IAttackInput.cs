using System;

namespace TestProject.Controllers
{
    public interface IAttackInput
    {
        event Action<int> TargetSelected; 
        void Activate(int[] targetIds);
        void Deactivate();
    }
}