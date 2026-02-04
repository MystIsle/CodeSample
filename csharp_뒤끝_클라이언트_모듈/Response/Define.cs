using System;
using System.Collections.Generic;
using BackEnd;
using Core.Networks.Error;

namespace Core.Networks.Response
{
    public class BaseResponse
    {
        public ResponseCode ResponseCode { get; set; }
        public StatusCode StatusCode { get; set; }
        public BackendReturnObject Bro { get; set; }
    }

    public interface IProcess<in TResponse> where TResponse : BaseResponse, new()
    {
        void OnProcess(TResponse response);
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class RowAttribute : System.Attribute
    {
        
    }

    public static class ResponseExtensions
    {
        public static bool IsServerError(this BaseResponse response) => response.Bro.IsServerError();
        public static bool IsSuccess(this BaseResponse response) => response.Bro.IsSuccess();
    }
}