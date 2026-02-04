using System.Collections;
using System.Collections.Generic;
using BackEnd;
using Sirenix.Utilities;

namespace Core.Networks.Error
{
    public static class Parser
    {
        private class StatusCodeComparer : IEqualityComparer<StatusCode>
        {
            public bool Equals(StatusCode x, StatusCode y)
            {
                return x == y;
            }

            public int GetHashCode(StatusCode obj)
            {
                return (int) obj;
            }
        }

        private class Match
        {
            public ResponseCode Code { get; }
            public string MessageKeyword { get; }
            public string ErrorCodeKeyword { get; }

            public Match(ResponseCode code, string messageKeyword, string errorCodeKeyword)
            {
                MessageKeyword = messageKeyword;
                ErrorCodeKeyword = errorCodeKeyword;
                Code = code;
            }

            public bool IsMatch(BackendReturnObject bro)
            {
                string errorCode = bro.GetErrorCode();
                string message = bro.GetMessage();

                if (ErrorCodeKeyword.IsNullOrWhitespace() == false)
                {
                    if (errorCode == null || errorCode.Contains(ErrorCodeKeyword) == false)
                    {
                        return false;
                    }
                }

                if (MessageKeyword.IsNullOrWhitespace() == false)
                {
                    if (message == null || message.Contains(MessageKeyword) == false)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private class Collection : IEnumerable<Match>
        {
            public List<Match> List { get; } = new List<Match>();

            public IEnumerator<Match> GetEnumerator() => List.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => List.GetEnumerator();

            public void Add(ResponseCode responseCode, string messageKeyword)
            {
                List.Add(new Match(responseCode, messageKeyword, string.Empty));
            }

            public void Add(ResponseCode responseCode, string messageKeyword, string errorCodeKeyword)
            {
                List.Add(new Match(responseCode, messageKeyword, errorCodeKeyword));
            }
        }

        private static readonly Dictionary<StatusCode, Collection> _statusCollection =
            new Dictionary<StatusCode, Collection>(new StatusCodeComparer())
            {
                {
                    StatusCode.BadRequest, new Collection
                    {
                        {ResponseCode.SDKException, "response process error"}
                    }
                },
                {
                    StatusCode.Unauthorized, new Collection
                    {
                        {ResponseCode.BadGoogleHash, "bad_google_hash"},
                        {ResponseCode.BadAccessToken, "bad accessToken"},
                        {ResponseCode.BadSignature, "bad signature"},
                        {ResponseCode.BadParameterSignature, "bad bad"},
                        {ResponseCode.ServerMaintenance, "maintenance"},
                        {ResponseCode.TimeDifference, "bad client_date"},
                        
                        //GuestLogin
                        {ResponseCode.NotExistId, "bad customId, "}
                    }
                },
                {
                    StatusCode.Forbidden, new Collection
                    {
                        {ResponseCode.TooManyRequests, "403 Forbidden"},
                        {ResponseCode.MaxActiveUserOnTestMode, "Forbidden Active User"},
                        {ResponseCode.BlockedUser, "blocked user"}
                    }
                },
                {
                    StatusCode.GatewayTimeOut, new Collection
                    {
                        {ResponseCode.GatewayTimeout, "504 Gateway"}
                    }
                },
                {
                    StatusCode.RequestTimeout, new Collection
                    {
                        {ResponseCode.ServerError, string.Empty, "ECONNABORTED"},
                        {ResponseCode.Timeout, string.Empty, "408"}
                    }
                },
                {
                    StatusCode.Conflict, new Collection
                    {
                        //CustomSignUp
                        {ResponseCode.DuplicatedCustomId, "customId", "DuplicatedParameterException"}
                    }
                },
                {
                    StatusCode.TooManyRequests, new Collection
                    {
                        {ResponseCode.DatabaseThroughputExceeded, "Exceeded"},
                        {ResponseCode.DatabaseThroughputUpdating, "Updating"}
                    }
                },
                {
                    StatusCode.ServiceUnavailable, new Collection
                    {
                        {ResponseCode.ServerUnavailable, "503 Service"}
                    }
                }
            };

        public static ResponseCode Parse(BackendReturnObject bro)
        {
            if (bro.IsSuccess())
            {
                return ResponseCode.Success;
            }

            string textStatus = bro.GetStatusCode();
            var statusCode = ToStatusCode(textStatus);
            if (statusCode == StatusCode.Undefined)
            {
                return ResponseCode.UndefinedError;
            }

            if (_statusCollection.TryGetValue(statusCode, out var collection) == false)
            {
                return ResponseCode.UndefinedError;
            }

            foreach (var match in collection.List)
            {
                if (match.IsMatch(bro))
                {
                    return match.Code;
                }
            }

            return ResponseCode.UndefinedError;
        }

        public static StatusCode ToStatusCode(string text)
        {
            if (int.TryParse(text, out var value) == false)
            {
                return StatusCode.Undefined;
            }

            return (StatusCode) value;
        }
    }
}