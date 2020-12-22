using QuickUnity.Core;
using UnityEngine;

namespace QuickUnity.Error
{
    [DisallowMultipleComponent]
    public abstract class ExceptionHandlerBase : SingletonMonoBehaviourBase<ExceptionHandlerBase>
    {
        protected virtual void OnEnable()
        {
            Application.logMessageReceived += HandleError;
            Application.logMessageReceivedThreaded += HandleErrorThreaded;
        }

        protected virtual void OnDisable()
        {
            Application.logMessageReceived -= HandleError;
            Application.logMessageReceivedThreaded -= HandleErrorThreaded;
        }

        /**
         * Handles exceptions raised by the main thread.
         */
        protected abstract void HandleError(string logString, string stackTrace, LogType type);

        /**
         * Handles exceptions raised outside the main thread.
         */
        protected abstract void HandleErrorThreaded(string logString, string stackTrace, LogType type);
    }
}
