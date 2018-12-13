using TestProject.Controllers;

namespace Tests
{
    public class BattleViewStub : IBattleView
    {
        public void Initialize(int[] playerSoldiers, int[] opponentSoldiers)
        {
        }

        public void Move(int id, MoveDirection direction)
        {
        }

        public void Kill(int id)
        {
        }
    }
}