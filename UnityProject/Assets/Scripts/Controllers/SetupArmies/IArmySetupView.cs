using System;

namespace TestProject.Controllers.SetupArmies
{
    public interface IArmySetupView
    {
        event Action<int, int> SetupCompleted;
        
        void Show(int playerMinSoldiers, int playerMaxSoldiers,int opponentMinSoldiers, int opponentMaxSoldiers);
        void Hide();
    }
}