using BackEnd;

namespace Core.Networks.Error
{
    public static class Extensions
    {
        public static ResponseCode ParseResponseCode(this BackendReturnObject bro) => Parser.Parse(bro);

        public static StatusCode ParseStatusCode(this BackendReturnObject bro) =>
            Parser.ToStatusCode(bro.GetStatusCode());
    }
}