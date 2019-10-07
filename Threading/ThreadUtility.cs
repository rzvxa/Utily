using System.Threading;
using System.Threading.Tasks;

namespace Utils.Threading
{
    public static class ThreadUtility
    {
        public static bool IsThreadLocked(object @object)
        {
            bool lockedBySomeOneElse = !Monitor.TryEnter(@object);
            if (!lockedBySomeOneElse) Monitor.Exit(@object);
            return lockedBySomeOneElse;
        }

        public static async Task<T> CancellableWait<T>
        (
             this Task<T> task,
             CancellationToken token
        )
        {
            await CancellableWait((Task) task, token);
            return await task;
        }

        public static async Task CancellableWait
        (
             this Task task,
             CancellationToken token
        )
        {
            while
            (
                 !task.IsCompleted &&
                 !task.IsFaulted &&
                 !task.IsCanceled
            )
            {
                await Task.Delay(10);
                token.ThrowIfCancellationRequested();
            }
        }
    }
}
