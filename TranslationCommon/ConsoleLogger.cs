using BepInEx.Logging;

namespace TranslationCommon
{
    public static class ConsoleLogger
    {
        public static readonly ManualLogSource LogSource;
        static ConsoleLogger()
        {
            LogSource = Logger.CreateLogSource(nameof(ConsoleLogger));
        }

        public static void Log(LogLevel level, object log)
        {
            LogSource.Log(level, log);
        }
        
        public static void LogDebug(object log)
        {
            LogSource.LogDebug(log);
        }
        
        public static void LogError(object log)
        {
            LogSource.LogError(log);
        }
        
        public static void LogFatal(object log)
        {
            LogSource.LogFatal(log);
        }
        
        public static void LogInfo(object log)
        {
            LogSource.LogInfo(log);
        }

        public static void LogWarning(object log)
        {
            LogSource.LogWarning(log);
        }
    }
}