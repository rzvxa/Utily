using UnityEngine.Networking;

namespace Utils.Unity.Runtime.Networking
{
    public static class UnityWebRequestExt
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp) =>
            new UnityWebRequestAwaiter(asyncOp);
    }
}
