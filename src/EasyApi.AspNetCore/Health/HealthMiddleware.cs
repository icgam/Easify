using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using EasyApi.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;

namespace EasyApi.AspNetCore.Health
{
    public sealed class HealthMiddleware
    {
        private const string JsonContentType = "application/json";
        private readonly IHostingEnvironment _host;
        private readonly RequestDelegate _next;

        public HealthMiddleware(RequestDelegate next, IHostingEnvironment host)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public async Task Invoke(HttpContext context)
        {
            var uri = new Uri(context.Request.GetDisplayUrl());
            if (uri.AbsolutePath.Equals("/health", StringComparison.OrdinalIgnoreCase))
            {
                var health = GenerateHealthInfo();
                context.Response.ContentType = JsonContentType;
                await context.Response.WriteAsync(health);
          
                return;
            }

            await _next.Invoke(context);
        }

        public string GenerateHealthInfo()
        {
            var version =
                Assembly.GetEntryAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion;

            var process = Process.GetCurrentProcess();
            var module = process.MainModule;
            var runtimeInfo = new
            {
                Pid = process.Id,
                Process = process.ProcessName,
                Release = module.FileName,
                Version = version,
                UpTime = $"{(DateTime.Now - process.StartTime).TotalSeconds:N2}s",
                Memory = $"{process.PrivateMemorySize64 / 1024:N2} MB",
                Cwd = _host.ContentRootPath
            };

            var hostInfo = new
            {
                Hostname = Dns.GetHostName(),
                OS = RuntimeInformation.OSDescription,
                Arch = RuntimeInformation.OSArchitecture
            };

            var healthInfo = new
            {
                Status = "Success",
                Service = new object(),
                Runtime = runtimeInfo,
                Host = hostInfo,
                Logs = new {
                    LatestErrors = LogsStore.Instance.Errors.OrderByDescending(e => e.LoggedAt).ToList(),
                    Messages = LogsStore.Instance.Logs.OrderByDescending(e => e.LoggedAt).ToList()
                }
            };
            return JsonConvert.SerializeObject(healthInfo, Formatting.Indented);
        }
    }
}