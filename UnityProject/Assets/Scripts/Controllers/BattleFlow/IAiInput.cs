using TestProject.Model;

namespace TestProject.Controllers
{
    public interface IAiInput
    {
        bool IsTrainRun { get; }
        void Initialize(BattleData battleData);

        void Done(ArmyType winner);
    }
}