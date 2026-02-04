using System;
using System.Threading;
using BackEnd;
using Core.DesignPatterns;
using Core.Networks.Apis;
using UnityEngine;

namespace Core.Networks
{
    public class BackEndProcessor : MonoSingleton<BackEndProcessor>
    {
        public static bool IsInitialized { get; private set; }
        
        
        protected override void OnAwake()
        {
            Backend.Initialize(OnBackendInitializeFinished);

            if (SendQueue.IsInitialize == false)
            {
                SendQueue.StartSendQueue(Api.LogLevel != LogLevel.Silent, exceptionEvent: OnExceptionEvent);
            }
        }

        private void OnBackendInitializeFinished()
        {
            if (Backend.IsInitialized == false)
            {
                if (Api.LogLevel != LogLevel.Silent)
                {
                    Debug.LogError("뒤끝 초기화 실패");
                }

                return;
            }

            IsInitialized = true;
        }

        protected override void OnDestroy()
        {
            if (SendQueue.IsInitialize)
            {
                while (SendQueue.UnprocessedFuncCount > 0)
                {
                    Thread.Sleep(100);
                    SendQueue.Poll();
                }

                SendQueue.StopSendQueue();
            }

            base.OnDestroy();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SendQueue.PauseSendQueue();
                return;
            }

            SendQueue.ResumeSendQueue();
        }

        private void OnApplicationQuit()
        {
            while (SendQueue.UnprocessedFuncCount > 0)
            {
                SendQueue.Poll();
            }

            if (SendQueue.IsInitialize)
            {
                SendQueue.StopSendQueue();
            }
        }

        private void Update()
        {
            if (IsInitialized == false)
            {
                return;
            }

            SendQueue.Poll();
        }

        private void OnExceptionEvent(Exception e)
        {
            Debug.LogError($"[SendQueue:OnExceptionEvent] {e.Message}\n{e.StackTrace}");
        }
    }
}