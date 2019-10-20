using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEngine;
using System;

namespace Utils.Unity.Runtime.Networking
{
    public static class RequestHelper
    {
        public static Task<DownloadHandler> DownloadAsync(string url) =>
            DownloadAsync(url, new DownloadHandlerBuffer());

        public static async Task<DownloadHandler> DownloadAsync(string url, DownloadHandler handler, bool throwOnDownloadFailure = false)
        {
            if(handler is null)
                throw new ArgumentNullException(nameof(handler));
            var req = new UnityWebRequest(url);
            req.downloadHandler = handler;
            await req.SendWebRequest();
            if(req.isNetworkError || req.isHttpError)
            {
                Debug.unityLogger.LogError(nameof(RequestHelper), req.error);
                if(throwOnDownloadFailure) throw new Exception(req.error);
            }
            return req.downloadHandler;
        }
    }
}
