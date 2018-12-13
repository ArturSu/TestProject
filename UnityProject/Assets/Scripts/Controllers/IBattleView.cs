using TestProject.Model;

namespace TestProject.Controllers
{
    public interface IBattleView
    {
        void Initialize(int[] playerSoldiers, int[] opponentSoldiers);
        void Move(int id, MoveDirection direction);
        void Kill(int id);
    }
}