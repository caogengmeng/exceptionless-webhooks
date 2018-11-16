using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging((ctx, builder) =>
                {
                    var env = ctx.HostingEnvironment;
                    IEnumerable<string> GetNlogConfigFiles()
                    {
                        yield return $"nlog.{env.EnvironmentName}.config";
                        yield return "nlog.config";
                    }

                    string GetNlogConfigFile()
                    {
                        return GetNlogConfigFiles().FirstOrDefault(f =>
                            env.ContentRootFileProvider.GetFileInfo(f).Exists);
                    }

                    var configFile = GetNlogConfigFile();
                    if (configFile == null)
                        throw new Exception("找不到nlog的配置文件");

                    env.ConfigureNLog(configFile);

                    builder
                        .AddConfiguration(ctx.Configuration)
                        .AddConsole()
                        .AddNLog();
                })
                .UseStartup<Startup>()
                .UseNLog()
                .Build();
    }
}