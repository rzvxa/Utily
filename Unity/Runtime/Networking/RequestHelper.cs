using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEngine;
using System;

namespace Utils.Unity.Runtime.Networking
{
    public static class RequestHelper
    {
        public static async Task<DownloadHandler> DownloadAsync(string url, DownloadHandler handler = null)
        {
            if(handler is null)
                handler = new DownloadHandlerBuffer();
            var req = new UnityWebRequest(url);
            req.downloadHandler = handler;
            await req.SendWebRequest();
            if(req.isNetworkError || req.isHttpError)
            {
                Debug.unityLogger.LogError(nameof(RequestHelper), req.error);
                throw new Exception(req.error);
            }
            return req.downloadHandler;
        }
    }
}
