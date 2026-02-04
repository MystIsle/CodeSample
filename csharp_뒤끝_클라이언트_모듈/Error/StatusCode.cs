namespace Core.Networks.Error
{
    public enum StatusCode
    {
        Undefined = 0,
        
        Ok = 200,
        Created = 201,
        
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        RequestTimeout = 408,
        Conflict = 409,
        TooManyRequests = 429,

        ServiceUnavailable = 503,
        GatewayTimeOut = 504,
    }
}