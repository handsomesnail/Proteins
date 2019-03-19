using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ZCore {

    public class HttpResponse {

        /// <summary>错误信息</summary>
        public string Error { get; private set; }

        /// <summary>返回码(0为正常返回)</summary>
        public long ErrorCode { get; private set; }

        /// <summary>返回数据</summary>
        public byte[] ResponseData { get; private set; }

        public HttpResponse(long responseCode, byte[] responseData, string error) {
            this.ErrorCode = responseCode;
            this.ResponseData = responseData;
            this.Error = error;
        }

    }

    /// <summary>网络请求相关</summary>
    /// 如果Http响应代码大于等于400 则isHttpError返回true
    /// 如果无法解析DNS地址,套接字错误或超出重定向
    public abstract class Service {

        protected IEnumerator Get(string url) {
            yield return Get(url, null);
        }

        protected IEnumerator Get(string url, Action<HttpResponse> completeCallback) {
            yield return Get(url, completeCallback, null);
        }

        /// <summary>Get请求</summary>
        protected IEnumerator Get(string url, Action<HttpResponse> completeCallback, Action<float> progressCallback) {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return Request(request, completeCallback, progressCallback);
        }


        protected IEnumerator Post(string url, Dictionary<string, string> formFields) {
            yield return Post(url, formFields, null);
        }

        protected IEnumerator Post(string url, Dictionary<string, string> formFields, Action<HttpResponse> completeCallback) {
            yield return Post(url, formFields, completeCallback, null);
        }

        /// <summary>Post请求</summary>
        protected IEnumerator Post(string url, Dictionary<string, string> formFields, Action<HttpResponse> completeCallback, Action<float> progressCallback) {
            UnityWebRequest request = UnityWebRequest.Post(url, formFields);
            yield return Request(request, completeCallback, progressCallback);
        }

        private IEnumerator Request(UnityWebRequest request, Action<HttpResponse> completeCallback, Action<float> progressCallback) {
            request.SendWebRequest();
            
            while (!request.isDone) {
                progressCallback?.Invoke(request.downloadProgress);
                yield return null;
            }
            progressCallback?.Invoke(1.0f);
            long ErrorCode = 0;
            if (request.isHttpError || request.isNetworkError) {
                Debug.LogAssertion(request.error);
                ErrorCode = request.responseCode;
            }
            HttpResponse response = new HttpResponse(ErrorCode, request.downloadHandler.data, request.error);
            completeCallback?.Invoke(response);
        }

    }

}
