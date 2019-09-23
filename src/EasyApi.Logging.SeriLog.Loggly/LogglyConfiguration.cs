namespace EasyApi.Logging.SeriLog.Loggly
{
    public sealed class LogglyConfiguration
    {
        public string ServerUrl { get; set; }
        public string CustomerToken { get; set; }
        public bool AllowLogLevelToBeControlledRemotely { get; set; }
        public string BufferBaseFilename { get; set; }
        public int? NumberOfEventsInSingleBatch { get; set; }
        public int? BatchPostingIntervalInSeconds { get; set; }
        public int? EventBodyLimitKb { get; set; }
        public int? RetainedInvalidPayloadsLimitMb { get; set; }
    }
}
