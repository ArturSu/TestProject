using TestProject.Model;

namespace TestProject.Controllers
{
    public interface IBattleView
    {
        void Initialize(BattleData battleData);
        void Move(int id, int positionX, int positionY);
        void Kill(int id);
    }
}