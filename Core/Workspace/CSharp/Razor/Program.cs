namespace Razor
{
    using System;
    using System.IO;
    using Allors.Services;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using NLog.Web;

    public class Program
    {
        private const string ConfigPath = "/config/core";

        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(new ConfigurationBuilder()
                    .AddCommandLine(args)
                    .Build())
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
                {
                    var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                    configurationBuilder.AddCrossPlatform(".", environmentName, true);
                    configurationBuilder.AddCrossPlatform(ConfigPath, environmentName);
                    configurationBuilder.AddCrossPlatform(Path.Combine(ConfigPath, hostingContext.HostingEnvironment.ApplicationName), environmentName);
                })
                .UseNLog();
    }
}
