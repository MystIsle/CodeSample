using System.Collections;
using System.Reflection;
using BackEnd;
using Core.Networks.Error;
using Core.Networks.Response;
using Sirenix.Utilities;
using UnityEngine;

namespace Core.Networks.Apis
{
    internal static class ApiUtility
    {
        public static TResponse Deserialize<TResponse>(BackendReturnObject bro) where TResponse : BaseResponse, new()
        {
            var response = DeserializeInternal<TResponse>(bro);

            response.ResponseCode = bro.ParseResponseCode();
            response.StatusCode = bro.ParseStatusCode();
            response.Bro = bro;

            return response;
        }

        private static TResponse DeserializeInternal<TResponse>(BackendReturnObject bro) where TResponse : BaseResponse, new()
        {
            var response = new TResponse();
            
            var type = typeof(TResponse);
            var rowType = typeof(RowAttribute);
            var listType = typeof(IList);
            
            foreach (var fieldInfo in type.GetFields())
            {
                var rowAttribute = fieldInfo.GetCustomAttribute(rowType);
                if (rowAttribute == null)
                {
                    continue;
                }

                if (fieldInfo.FieldType.IsAssignableFrom(listType))
                {
                    if (bro.HasRows() == false)
                    {
                        return response;
                    }
                    
                    var jsonData = BackendReturnObject.Flatten(bro.Rows());
                    var rowPacket = LitJson.JsonMapper.ToObject(jsonData.ToJson(), fieldInfo.FieldType);
                    fieldInfo.SetValue(response, rowPacket);
                }
                else
                {
                    var jsonData = bro.GetReturnValuetoJSON();
                    if (jsonData.ContainsKey("row") == false)
                    {
                        return response;
                    }

                    var rowData = jsonData["row"];
                    if (rowData == null || rowData.Count <= 0)
                    {
                        return response;
                    }

                    string json = rowData.ToJson();
                    var rowPacket = LitJson.JsonMapper.ToObject(json, fieldInfo.FieldType);
                    fieldInfo.SetValue(response, rowPacket);
                    return response;
                }
            }

            return response;
        }
    }
}