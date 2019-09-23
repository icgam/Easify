using EasyApi.Extensions;

namespace EasyApi.Logging
{
    public sealed class LogsStore
    {
        private const int BufferSize = 50;
        private const int ErrorsBufferSize = 5;
        private static volatile LogsStore _instance;
        private static readonly object SyncRoot = new object();

        private LogsStore()
        {
            Logs = new CircularBuffer<LogMessage>(BufferSize);
            Errors = new CircularBuffer<LogMessage>(ErrorsBufferSize);
        }

        public static LogsStore Instance
        {
            get
            {
                if (_instance == null)
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new LogsStore();
                    }

                return _instance;
            }
        }

        public CircularBuffer<LogMessage> Logs { get; }

        public CircularBuffer<LogMessage> Errors { get; }
    }
}