using Serilog.Core;

namespace EasyApi.Logging.SeriLog
{
    public static class LoggingLevelSwitchProvider
    {
        private static volatile LoggingLevelSwitch _instance;
        private static readonly object SyncRoot = new object();

        public static LoggingLevelSwitch Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new LoggingLevelSwitch();
                }

                return _instance;
            }
        }
    }
}
