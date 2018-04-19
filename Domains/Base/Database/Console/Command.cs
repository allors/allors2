using System.IO;

namespace Allors.Console
{
    using System.Data;
    using System.Linq;

    using Allors.Adapters.Object.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using ObjectFactory = Allors.ObjectFactory;

    public abstract class Command
    {
        protected Command()
        {
            this.Configuration = new ConfigurationBuilder()
                .AddJsonFile(@"appSettings.json")
                .Build();
            this.ObjectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User));
        }

        protected IConfigurationRoot Configuration { get; }

        protected ObjectFactory ObjectFactory { get; }

        protected string DataPath => this.Configuration["dataPath"];

        public abstract void Execute();

        protected IDatabase CreateDatabase(IsolationLevel isolationLevel = IsolationLevel.Snapshot)
        {
            var configuration = new Configuration
                                    {
                                        ConnectionString = this.Configuration["allors"],
                                        ObjectFactory = this.ObjectFactory,
                                        IsolationLevel = isolationLevel,
                                        CommandTimeout = 0
                                    };
            
            var directoryInfo = new DirectoryInfo(".");
            while (directoryInfo != null && directoryInfo.GetDirectories("Server").Length == 0)
            {
                directoryInfo = directoryInfo.Parent;
            }

            directoryInfo = directoryInfo?.GetDirectories("Server").FirstOrDefault();

            var services = new ServiceCollection();
            services.AddAllors(new ServiceConfig
            {
                Directory = directoryInfo,
                ApplicationName = "Server"
            });
            var serviceProvider = services.BuildServiceProvider();

            var database = new Database(serviceProvider, configuration);

            return database;
        }
    }
}
