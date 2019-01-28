using Microsoft.Extensions.Configuration;

namespace Allors.Server
{
    using System;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;

    using NLog.Web;

    public class Program
    {
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
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    const string FileName = @"apps.appSettings.json";
                    var userSettings = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/allors/{FileName}";
                    var systemSettings = $@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}/allors/{FileName}";

                    config.AddJsonFile(systemSettings, true);
                    config.AddJsonFile(userSettings, true);
                })
                .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Trace);
                    })
                .UseNLog();
    }
}
