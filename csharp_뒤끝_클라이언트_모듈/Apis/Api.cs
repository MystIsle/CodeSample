namespace Core.Networks.Apis
{
    public enum LogLevel
    {
        Silent,
        OnlyError,
        Full,
    }
    
    public static partial class Api
    {
        public static LogLevel LogLevel { get; } = LogLevel.Full;
    }
}