using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Controllers.Core
{
    public abstract class ControllerBase
    {
        private readonly List<ControllerBase> _childControllers = new List<ControllerBase>();

        private CancellationTokenSource _stopToken;

        public ControllerState State { get; private set; }

        protected ControllerFactory ControllerFactory { get; }
        protected string ControllerName { get; }
        protected bool IsControllerAlive => !_stopToken.IsCancellationRequested;

        protected ControllerBase(ControllerFactory controllerFactory)
        {
            ControllerFactory = controllerFactory;
            State = ControllerState.Created;
            ControllerName = GetType().Name;
        }

        public void Initialize(object arg)
        {
            SetArg(arg);
        }
       
        public void Stop()
        {
            switch (State)
            {
                case ControllerState.Created:
                    throw new InvalidOperationException("Controller can not be stopped from current state");
                case ControllerState.Running:

                    _stopToken.Cancel();

                    foreach (var child in _childControllers.ToArray())
                    {
                        if (child.State == ControllerState.Running)
                        {
                            child.Stop();
                        }
                    }

                    OnStop();
                    RemoveEventHandlers();

                    State = ControllerState.Stopped;
                    break;
            }
        }

        public void AddController(ControllerBase controller)
        {
            _childControllers.Add(controller);

            if (State == ControllerState.Running)
            {
                controller.Start();
            }
        }

        public void RemoveController(ControllerBase controller)
        {
            controller.Stop();
            _childControllers.Remove(controller);
        }

        public T CreateAndStart<T>(object arg = null)
            where T : ControllerBase
        {
            var controller = ControllerFactory.Create<T>(arg);
            AddController(controller);
            controller.Start();
            return controller;
        }

        public T CreateAndStart<T>(Type type, object arg = null)
            where T : ControllerBase
        {
            var controller = ControllerFactory.Create<T>(type, arg);
            AddController(controller);
            controller.Start();
            return controller;
        }

        public void Start()
        {
            if (State != ControllerState.Running && State != ControllerState.Starting)
            {
                _stopToken = new CancellationTokenSource();


                State = ControllerState.Starting;

                AddEventHandlers();
                OnStart();

                foreach (var child in _childControllers.ToArray())
                {
                    child.Start();
                }

                State = ControllerState.Running;
            }
        }

        protected abstract void OnStart();

        protected virtual void SetArg(object arg)
        {
        }

        protected virtual void OnStop()
        {
        }

        protected virtual void AddEventHandlers()
        {
        }

        protected virtual void RemoveEventHandlers()
        {
        }
    }
}