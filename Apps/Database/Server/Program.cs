namespace Allors.Server
{
    using System.IO;
    using System.Runtime.InteropServices;
    using Allors.Services;
    using System;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
    using NLog.Web;

    public class Program
    {
        private static bool IsOsx => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

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
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
                {
                    const string root = "/config/apps";
                    var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                    configurationBuilder.AddCrossPlatform(".", environmentName, true);
                    configurationBuilder.AddCrossPlatform(root, environmentName);
                    configurationBuilder.AddCrossPlatform(Path.Combine(root, "server"), environmentName);
                })
                .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Trace);
                    })
                .UseNLog();
    }
}
