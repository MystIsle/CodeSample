namespace Core.Networks.Error
{
    public enum ResponseCode
    {
        // - Common -

        // 안드로이드 OS 환경에서 Client(게임)와 Server(뒤끝 콘솔) 간 구글 해시키가 일치하지 않는 경우
        BadGoogleHash,

        // Access Token 이 올바르지 않은 경우
        BadAccessToken,

        // 서버가 정상적으로 작동하지 않는 경우
        ServerUnavailable,

        // Client(게임)와 Server(뒤끝 콘솔) 간 시그니처가 일치하지 않는 경우
        BadSignature,

        // 뒤끝서버로 요청한 함수의 Param 인자 값 내부에 소수점 이하 14자리 이상의 double 형 데이터가 포함된 경우
        // 뒤끝서버로 요청한 함수의 Param 인자 값 내부에 4depth를 이상의 Dictionary 타입의 데이터가 포함된 경우
        BadParameterSignature,

        // 한 클라이언트(동일ip)에서 너무 많은 요청을 보낸 경우
        // - 해당 에러가 발생한 클라이언트는 5분동안 요청을 보낼 수 없습니다.
        TooManyRequests,

        // 서버 상태 점검중
        ServerMaintenance,

        // 뒤끝콘솔에서 서버설정이 테스트 모드인데 10명을 초과하는 계정의 회원가입/로그인 시도를 한 경우
        MaxActiveUserOnTestMode,

        // 서버에서 타임아웃 오류가 발생한 경우(최대 120초)
        GatewayTimeout,

        // 서버에서 오류가 발생한 경우
        ServerError,

        // 서버와 클라이언트의 시간이 UTC+9(한국시간) 기준 10분 이상 차이가 나는 경우
        TimeDifference,
        
        // SDK에서 서버로 부터 요청을 받은 후 예외가 발생한 경우
        SDKException,
        
        // 클라이언트가 요청을 보낸 후 타임아웃(서버에서 응답이 늦거나, 네트워크 등이 끊겨 있는 경우) 오류가 발생한 경우(최대 100초)
        Timeout,
        
        // 데이터베이스 할당량을 초과한 경우
        DatabaseThroughputExceeded,
        
        // 데이터베이스 할당량 업데이트 중인 경우
        DatabaseThroughputUpdating,

        //알 수 없는 에러
        UndefinedError,


        // - Success -
        Success = 0,

        // - Contents -
        
        // GuestLogin
        NotExistId,
        BlockedUser,
        
        // CustomSignUp
        DuplicatedCustomId
    }
}