using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.CompilerServices;

namespace Utils.Unity.Runtime.Networking
{
    public class UnityWebRequestAwaiter : INotifyCompletion
    {
        private UnityWebRequestAsyncOperation _asyncOperation;
        private Action _continuation;


        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
        {
            _asyncOperation = asyncOp;
            asyncOp.completed += OnRequestCompleted;
        }

        public bool IsCompleted => _asyncOperation.isDone;

        public void GetResult() {}

        public void OnCompleted(Action continuation)
        {
            if(IsCompleted)
                OnRequestCompleted(null);
            else _continuation = continuation;
        }

        private void OnRequestCompleted(AsyncOperation obj) =>
            _continuation?.Invoke();
    }
}
