using System.Threading.Tasks;
using System;
using UnityEngine;

namespace Utils.Unity.Runtime
{
    public static class AsyncHelper
    {
        public static T RunSynchronous<T>(this Task<T> action)
        {
            T res = default(T);
            Task.Run(async () => res = await action).GetAwaiter().GetResult();
            return res;
        }

        public static void RunSynchronous(this Task action)
        {
            Task.Run(async () => await action).GetAwaiter().GetResult();
        }

        public static async void FireAndForget(this Task action)
        {
            try
            {
                await action;
            }
            catch(Exception e)
            {
                Debug.unityLogger.LogException(e);
            }
        }
    }
}
