using System;

namespace TestProject.Controllers
{
    public interface IMoveInput
    {
        event Action<Tuple<int, int>> TileSelected;
        InputType InputType { get; }
        void Activate(Tuple<int, int, MoveDirection>[] tiles, int soldierId);
        void Deactivate();
    }
}