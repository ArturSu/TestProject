namespace Controllers.Core
{
    public class ControllerResultBase
    {
        public ControllerBase Controller { get; }

        public ControllerResultBase(ControllerBase controller)
        {
            Controller = controller;
        }
    }
}