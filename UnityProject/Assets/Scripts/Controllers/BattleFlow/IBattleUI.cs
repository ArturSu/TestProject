using System;

namespace TestProject.Controllers
{
    public interface IBattleUI
    {
        event Action LeavePressed;
        event Action ContinuePressed;
        
        void Activate(bool showLog);
        void Deactivate();
        void AddLog(string message);
        void ShowResultMessage(string message);
    }
}