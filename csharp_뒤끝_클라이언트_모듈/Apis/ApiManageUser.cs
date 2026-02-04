using BackEnd;
using Core.Networks.Error;
using Core.Networks.Response;

namespace Core.Networks.Apis
{
    using LoginGuest = Handler<ResponseGuestLogin>;
    using GetUserInfo = Handler<ResponseGetUserInfo>;

    public static partial class Api
    {
        public static string GuestId => Backend.BMember.GetGuestID();
        
        public static LoginGuest LoginGuest { get; } = new LoginGuest()
            .SetRequest(Backend.BMember.GuestLogin);

        public static void DeleteGuestId() => Backend.BMember.DeleteGuestInfo();

        public static GetUserInfo GetUserInfo { get; } = new GetUserInfo()
            .SetRequest(Backend.BMember.GetUserInfo);
    }
}