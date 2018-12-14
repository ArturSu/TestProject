using System;

namespace TestProject.Controllers
{
    public interface IMoveInput
    {
        event Action<Tuple<int, int>> TileSelected; 
        void Activate(Tuple<int, int>[] tiles);
        void Deactivate();
    }
}