namespace EasyApi.Logging.SeriLog.Seq
{
    public sealed class SeqConfiguration
    {
        public string ServerUrl { get; set; }
        public string ApiKey { get; set; }
        public bool AllowLogLevelToBeControlledRemotely { get; set; }
    }
}
