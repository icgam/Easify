## Logging ([Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework))

Right before starting logging anything, It workths to have a look on a mini guideline on what you should log and how you should do it. Here is the [link](https://github.com/icgam/Guidelines/blob/master/docs/development/logging.md)

Logging is always tricky to configure. Its even more tricky to make logs consistant across different apps and when it comes to integrating with log aggregators and capturing the same set of metrics your head might start spinning :) Well we have a solution for all of it! One liner log configuration:

```csharp

    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.Run<Startup>(s =>  s.ConfigureLogger<Startup>());
        }
    }

```

Ok, dont get to excited, this has NO integration with any of the cloud providers yet, but it does define a consistant file format, as well as consistant set of variables that are persisted to the logs. Currently default configuration saves logs to: **$approot$\logs** in **DEV** environment and **D:\logs** all other environments. Log path may be optionally be provided in either ***.json** or ***.{ENV}.json** files, if non of the configured defaults work for your scenario.

### How to provide environment

The environment is coming from *appSettings.json* which from Application:Environment. The values are user-defined but to be in-line with .Net Core environments they can be (Development, Integration, Uat, Staging and Production).  

**Notes**:

* If the value is not explicitly provided in Application section of appsettings.json it will be provided by environment from ASPNETCORE_ENVIRONMENT variable.

* The required values have been defined in OctopusDeploy > Valiables > GlobalDns. It automatically get filled by Octopus if the setting is available in the process. 

The pattern we use for log file is: **"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{MachineName}] [{EnvironmentUserName}] [{ProcessId}] [{UserName}] [{CorrelationId}] [{ThreadId}] [{Level}] {Message}{NewLine}{Exception}"**

To write logs we use native ASP.NET Core logging framework, in order to get a logger instance we need to inject it into any component we want write some logs from:

```csharp
public class MyService
{
    private readonly ILogger<MyService> _log;

    public MyService(ILogger<MyService> log)
    {
        _log = log;
    }
    public void DoWork()
    {
        _log.LogInformation("I am doing some great work here:)");
    }
}
```

Underlying logging framework is [Serilog](https://serilog.net/), both native logger and Serilog supports logging structured events. Please read more about this at [https://serilog.net/](https://serilog.net/).

There is one setting we are of interest in the config for basic logging and that is the default logging level which is controlled by manipulating **MinimumLevel** value and can be found here:

```json
{
  "Logging": {
    "IncludeScopes": false,
    "Serilog": {
      "MinimumLevel": "Verbose"
    }
  }
}
```

If you are interested about **IncludeScopes** setting, you may wanna refer to [ASP.NET Core Logging Docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging) for a in-depth explanation.

### Extra Features

We have a couple of great features that comes built in with the framework. For example we can check latest 50 logs and last 5 errors by going to **http://{MY-COMPANY-URL}/diagnostics/logs**. Here is an example of what you may see:

```json
{
  "Status": "Success",
  "Service": {},
  "Runtime": {
    "Pid": 61020,
    "Process": "dotnet",
    "Release": "C:\\Program Files\\dotnet\\dotnet.exe",
    "Version": "1.0.0",
    "UpTime": "9.23s",
    "Memory": "108,456.00 MB",
    "Cwd": "c:\\Work\\ICG-GitHub\\Template.DotNetCore.WebAPI\\Template.WebAPI"
  },
  "Host": {
    "Hostname": "ICGLDNPPNGEN22",
    "OS": "Microsoft Windows 10.0.15063 ",
    "Arch": 1
  },
  "Logs": {
    "LatestErrors": [],
    "Messages": [
      {
        "LoggedAt": "2017-05-22T14:10:35.9872852+01:00",
        "Level": "Information",
        "Message": "2017-05-22 14:10:35.987 +01:00 [Information] Request finished in 256.2077ms 200 \r\n"
      }
    ]
  }
}
```

Another really cool feature is ability to change the level of logging for the API on the fly. To achieve this you need to **POST** a message with desired logging level to **http://{MY-COMPANY-URL}/diagnostics/logs** endpoint. Available logging levels are:

* *Verbose,*
* *Debug,*
* *Information,*
* *Warning,*
* *Error,*
* *Fatal*

Here is the **POST** message example:

```json
{
  "logginglevel": "Debug"
}
```

### Logging Action Filters

Solution comes preconfigured with an **ActionFilter** which intercepts every call to ANY app controller and logs entering/exiting events with a list of arguments passed to the action. It is configured to output messages at *DEBUG* and more detailed messages at *TRACE* logging levels. Great little addition in case we need to troubleshoot our API's.

Ok so we are DONE with the boring boilerplate stuff that all of us have seen 1000's of times before! Its time for something more exciting ... The cloud! :) Currently we support following log aggregators:

* [Seq](https://getseq.net/) - On prem solution for log aggregation (Can be hosted on a VM in the cloud)
* [Loggly](https://www.loggly.com/) - Cloud based log aggregator / analytics platform
* [LogEntries](https://logentries.com/) - Cloud based log aggregator / analytics platform / super fast!

### Seq

Prerequiste for **Seq** is obviously installation of the actual server, but after that is complete, the rest is super simple. Actually its as simple as modifying your log configuration as shown below:

```csharp
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.Run<Startup>(s => s.ConfigureLogger<Startup>(c =>
                c.UseSeq("https://seq.server.com").WithApiKey("api-key")));
        }
    }
```

This will allow you to use **Seq** in its most basic configuration, given that you have provided correct **server URL** and working **API-KEY**.

There is one really cool feature that **Serilog** gives us, which is runtime log level manipulation. Luckily **Seq** takes advantage of it, so as we! If you wish to enabled this use the following code snippet:

```csharp
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.Run<Startup>(s => s.ConfigureLogger<Startup>(c =>
                c.UseSeq("https://seq.server.com")
                .WithApiKey("api-key")
                .EnableLogLevelControl()));
        }
    }
```

This last setting allows us to change logging level on the fly from **Seq** admin UI. Awesome! By now you must be questioning these *HARDCODED* settings approach and you are comletetly within your rights to do so, which is why we have an alternative option of loading all those settings from either **\*.json** or **\*.{ENV}.json** files. To achieve that we need to do two things:

* Configure our logger to use *Seq* configuration

```csharp
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.Run<Startup>(s => s.ConfigureLogger<Startup>(c =>
                c.UseSeq(s.Configuration.GetSection("Logging:Seq"))));
        }
    }
```

* Add *Seq* configuration to your Logging config section

```json
{
  "Logging": {
    "IncludeScopes": false,
    "Serilog": {
      "MinimumLevel": "Verbose"
    },
    "Seq": {
      "ServerUrl": "http://seq.server.com",
      "ApiKey": "API-KEY",
      "AllowLogLevelToBeControlledRemotely": true
    }
  }
}
```

### Loggly

In order to use Loggly we do need to create an account at [https://www.loggly.com/](https://www.loggly.com/). Once thats done we will need to retrieve **Loggly server url** and **customer token**. The simplest configuration looks like is shown below:

```csharp
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.Run<Startup>(s => s.ConfigureLogger<Startup>(c => c.UseLoggly("https://logs-01.loggly.com/")
                .WithCustomerToken("CUSTOMER-TOKEN")
                .BufferLogsAt("logs\\logs-buffer")));
        }
    }
```

Perhaps one of more interesting parameters is **BufferLogsAt**, which expect you to specify either *relative* or *absolute* path for buffering logs and a pattern for a file name. For example the current configuration that points to *logs\\logs-buffer* will buffer logs in *%APPLICATION_ROOT%\logs\logs-bufferXXXXX* files. Loggly will roll these files and will cleanup after all buffered logs are uploaded.

Loggly also supports runtime log level manipulation, which is one of the great features of **Serilog**. In order to achieve this please use the following code snippet:

```csharp
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.Run<Startup>(s => s.ConfigureLogger<Startup>(c => c.UseLoggly("https://logs-01.loggly.com/")
                .WithCustomerToken("CUSTOMER-TOKEN")
                .BufferLogsAt("logs\\logs-buffer")
                .EnableLogLevelControl()));
        }
    }
```

Now while this is great for quick and 'dirty' setup, it is most likekly we want to run different configurations for our app per environment. For this we can load all the settings from either **\*.json** or **\*.{ENV}.json** files. To achieve that we need to do two things:

* Configure our logger to use *Loggly* configuration

```csharp
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.Run<Startup>(s => s.ConfigureLogger<Startup>(
                c => c.UseLoggly(s.Configuration.GetSection("Logging:Loggly"))
            ));
        }
    }
```

* Add *Loggly* configuration to your Logging config section

```json
{
  "Logging": {
    "IncludeScopes": false,
    "Serilog": {
      "MinimumLevel": "Verbose"
    },
    "Loggly": {
      "ServerUrl": "https://logs-01.loggly.com/",
      "CustomerToken": "CUSTOMER-TOKEN",
      "BufferBaseFilename": "logs\\loggly-buffer",
      "AllowLogLevelToBeControlledRemotely": true
    }
  }
}
```

This is the minimum configuration that is required. For the omited config values, defaults will be used. If you are wondering what other options are available, please take a look at the snippet below

```json
{
  "Logging": {
    "IncludeScopes": false,
    "Serilog": {
      "MinimumLevel": "Verbose"
    },
    "Loggly": {
      "ServerUrl": "https://logs-01.loggly.com/",
      "CustomerToken": "CUSTOMER-TOKEN",
      "BufferBaseFilename": "logs\\loggly-buffer",
      "NumberOfEventsInSingleBatch": 50,
      "BatchPostingIntervalInSeconds": 2,
      "EventBodyLimitKb": 10,
      "RetainedInvalidPayloadsLimitMb": 100,
      "AllowLogLevelToBeControlledRemotely": true
    }
  }
}
```

[Back to Index](https://github.com/icgam/ICG.DotNetCore.Framework)
