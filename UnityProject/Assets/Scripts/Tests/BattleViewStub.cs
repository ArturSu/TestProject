using TestProject.Controllers;
using TestProject.Model;

namespace Tests
{
    public class BattleViewStub : IBattleView
    {
        public void Initialize(BattleData battleData)
        {
        }

        public void Hide()
        {
        }

        public void Move(int id, int positionX, int positionY)
        {
        }

        public void Kill(int id)
        {
        }
    }
}