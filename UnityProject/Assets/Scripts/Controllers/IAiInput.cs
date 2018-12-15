using TestProject.Model;

namespace TestProject.Controllers
{
    public interface IAiInput
    {
        void Initialize(BattleData battleData);

        void Done();
    }
}