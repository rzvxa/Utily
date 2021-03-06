using UnityEngine;
using System.Threading.Tasks;
using Utils.Unity.Runtime;

namespace Utils.Unity.Runtime.Threading
{
    public abstract class AsyncMonoBehaviour : MonoBehaviour
    {
        #region Unity3D native engine events
        protected virtual void Awake() => AsyncAwake().FireAndForget();
        protected virtual void Start() => AsyncStart().FireAndForget();
        protected virtual void Update() => AsyncUpdate().FireAndForget();
        protected virtual void OnEnable() => AsyncOnEnable().FireAndForget();
        protected virtual void OnDisable() => AsyncOnDisable().FireAndForget();
        protected virtual void OnApplicationQuit() => AsyncOnApplicationQuit().FireAndForget();
        protected virtual void OnApplicationPause(bool pause) => AsyncOnApplicationPause(pause).FireAndForget();
        #endregion

        #region Async engine events
        protected virtual Task AsyncAwake() => Task.CompletedTask;
        protected virtual Task AsyncStart() => Task.CompletedTask;
        protected virtual Task AsyncUpdate() => Task.CompletedTask;
        protected virtual Task AsyncOnEnable() => Task.CompletedTask;
        protected virtual Task AsyncOnDisable() => Task.CompletedTask;
        protected virtual Task AsyncOnApplicationQuit() => Task.CompletedTask;
        protected virtual Task AsyncOnApplicationPause(bool pause) => Task.CompletedTask;
        #endregion
    }
}
