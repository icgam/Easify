namespace EasyApi.Configurations
{
    public class AppInfo
    {
        public AppInfo(string name, string version, string environment)
        {
            Name = name;
            Version = version;
            Environment = environment;
        }

        public string Name { get; }
        public string Version { get; }
        public string Environment { get; }
    }
}