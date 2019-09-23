using EasyApi.AspNetCore;
using EasyApi.Logging.SeriLog.Loggly;
using Microsoft.AspNetCore.Hosting;

namespace EasyApi.Sample.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.Run<Startup>(s =>
            {
                if (s.Environment.IsDevelopment() || s.Environment.IsEnvironment("INT"))
                    return s.ConfigureLogger<Startup>();

                return s.ConfigureLogger<Startup>(c => c.UseLoggly(s.Configuration.GetSection("Logging:Loggly")));
            }, args);
        }
    }
}
