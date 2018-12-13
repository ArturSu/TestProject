using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Controllers.Core
{
    public abstract class ControllerWithResultBase<T> : ControllerBase
    {
        private TaskCompletionSource<T> _resultSource;

        protected ControllerWithResultBase(ControllerFactory controllerFactory) : base(controllerFactory)
        {
        }

        public async void WaitForResult(Func<T, Task> processTaskFunc)
        {
            try
            {
                if (!IsControllerAlive)
                {
                    return;
                }

                var result = await _resultSource.Task.ConfigureAwait(true);

                if (IsControllerAlive)
                {
                    await processTaskFunc(result);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error during controller navigation in {ControllerName}. Exc: {e}");
                throw;
            }
        }
        
        public Task<T> GetProcessedTask()
        {
            return _resultSource.Task;
        }

        protected override void OnStart()
        {
            _resultSource = new TaskCompletionSource<T>();
        }

        protected void Complete(T result)
        {
            _resultSource.TrySetResult(result);
        }

        protected void Cancel()
        {
            _resultSource.TrySetCanceled();
        }

        protected void Fail(Exception e)
        {
            _resultSource.TrySetException(e);
        }
    }

    public abstract class ControllerWithResultBase : ControllerWithResultBase<ControllerResultBase>
    {
        protected ControllerWithResultBase(ControllerFactory controllerFactory) : base(controllerFactory)
        {
        }
    }
}
