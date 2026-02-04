using System;
using System.Collections;
using System.Reflection;
using BackEnd;
using Core.Networks.Response;
using UnityEngine;

namespace Core.Networks.Apis
{
    public delegate void OnResponseProcess<TResponse>(ref TResponse response) where TResponse : BaseResponse;

    public abstract class BaseHandler<TResponse> where TResponse : BaseResponse, new()
    {
        private static void LogResponse(TResponse response)
        {
            if (Api.LogLevel == LogLevel.Silent)
            {
                return;
            }

            var result = response.Bro;
            var responseCode = response.ResponseCode;

            if (result.IsServerError())
            {
                Debug.LogError($"[{nameof(Api)}:ServerError]\n" +
                               $"statusCode: {result.GetStatusCode()}\n" +
                               $"errorCode: {result.GetErrorCode()}\n" +
                               $"message: {result.GetMessage()}\n" +
                               $"responseCode: {responseCode}\n" +
                               $"returnValue:{result.GetReturnValue()}"
                );
            }
            else if (result.IsSuccess() == false)
            {
                Debug.LogError($"[{nameof(Api)}:Error]\n" +
                               $"statusCode: {result.GetStatusCode()}\n" +
                               $"errorCode: {result.GetErrorCode()}\n" +
                               $"message: {result.GetMessage()}\n" +
                               $"responseCode: {responseCode}\n" +
                               $"returnValue:{result.GetReturnValue()}"
                );
            }
            else if (Api.LogLevel == LogLevel.Full)
            {
                Debug.Log($"[{nameof(Api)}:ReturnValue]\n" +
                          $"responseCode: {responseCode}\n" +
                          $"{result.GetReturnValue()}");
            }
        }

        public bool IsRunning { get; protected set; }
        public TResponse Response { get; protected set; }
        
        protected abstract MethodInfo RequestMethodInfo { get; }
        
        private Action<TResponse> _onResponse;


        private TResponse Process(BackendReturnObject result)
        {
            var response = ApiUtility.Deserialize<TResponse>(result);
            LogResponse(response);
            TryProcessWhenServerError(response);

            if (response is IProcess<TResponse> process)
            {
                process.OnProcess(response);
            }

            return response;
        }

        private void TryProcessWhenServerError(TResponse response)
        {
            if (response.IsServerError() == false)
            {
                return;
            }

            //TODO: 서버 공통 에러에 대한 핸들링
        }

        public void Request(Action<TResponse> onResponse)
        {
            if (IsRunning)
            {
                Debug.LogError($"[ApiHandler] cannot request api. {RequestMethodInfo.Name} is running.");
                return;
            }

            RequestInternal(onResponse);
        }

        public IEnumerator RoutineRequest(Action<TResponse> onResponse = null)
        {
            if (IsRunning)
            {
                Debug.LogError($"[ApiHandler] cannot request api. {RequestMethodInfo.Name} is running.");
                yield break;
            }

            RequestInternal(onResponse);
            while (IsRunning)
            {
                yield return null;
            }
        }

        protected abstract void QueueRequest();

        private void RequestInternal(Action<TResponse> onResponse)
        {
            IsRunning = true;
            _onResponse = onResponse;
            QueueRequest();
        }
        
        protected void OnResponse(BackendReturnObject result)
        {
            IsRunning = false;

            Response = Process(result);
            _onResponse?.Invoke(Response);
            IsRunning = false;
        }
    }

    public sealed class Handler<TResponse> : BaseHandler<TResponse>
        where TResponse : BaseResponse, new()
    {
        protected override MethodInfo RequestMethodInfo => _requestMethod.Method;

        private Func<BackendReturnObject> _requestMethod;


        public Handler<TResponse> SetRequest(Func<BackendReturnObject> requestMethod)
        {
            _requestMethod = requestMethod;
            return this;
        }

        protected override void QueueRequest() => SendQueue.Enqueue(_requestMethod, OnResponse);
    }

    public sealed class Handler<T1, TResponse> : BaseHandler<TResponse>
        where TResponse : BaseResponse, new()
    {
        public T1 Param1 { get; private set; }
        
        protected override MethodInfo RequestMethodInfo => _requestMethod.Method;

        private Func<T1, BackendReturnObject> _requestMethod;


        public Handler<T1, TResponse> SetRequest(Func<T1, BackendReturnObject> requestMethod)
        {
            _requestMethod = requestMethod;
            return this;
        }

        public Handler<T1, TResponse> SetParam(T1 param1)
        {
            Param1 = param1;
            return this;
        }

        protected override void QueueRequest() => SendQueue.Enqueue(_requestMethod, Param1, OnResponse);
    }

    public sealed class Handler<T1, T2, TResponse> : BaseHandler<TResponse>
        where TResponse : BaseResponse, new()
    {
        public T1 Param1 { get; private set; }
        public T2 Param2 { get; private set; }
        
        protected override MethodInfo RequestMethodInfo => _requestMethod.Method;

        private Func<T1, T2, BackendReturnObject> _requestMethod;


        public Handler<T1, T2, TResponse> SetRequest(Func<T1, T2, BackendReturnObject> requestMethod)
        {
            _requestMethod = requestMethod;
            return this;
        }

        public Handler<T1, T2, TResponse> SetParam(T1 param1, T2 param2)
        {
            Param1 = param1;
            Param2 = param2;
            return this;
        }

        protected override void QueueRequest() => SendQueue.Enqueue(_requestMethod, Param1, Param2, OnResponse);
    }
}