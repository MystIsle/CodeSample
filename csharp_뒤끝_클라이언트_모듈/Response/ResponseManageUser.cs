using BackEnd;
using Core.Networks.Error;
using Core.Networks.Table;

namespace Core.Networks.Response
{
    public class ResponseGuestLogin : BaseResponse, IProcess<ResponseGuestLogin>
    {
        public bool isSignUp;

        public void OnProcess(ResponseGuestLogin response)
        {
            if (response.StatusCode == StatusCode.Created)
            {
                response.isSignUp = true;
            }
        }
    }

    public class ResponseGetUserInfo : BaseResponse
    {
        [Row] public UserInfo info;
    }
}