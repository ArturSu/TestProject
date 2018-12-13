using System;

namespace TestProject.Controllers
{
    public interface IMoveInput
    {
        event Action<MoveDirection> DirectionSelected; 
        void Activate(MoveDirection[] directions);
        void Deactivate();
    }
}