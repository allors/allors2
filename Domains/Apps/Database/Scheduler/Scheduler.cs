namespace Allors.Scheduler
{
    using System.Data;
    using System.IO;

    using Allors;
    using Allors.Adapters.Object.SqlClient;
    using Allors.Adapters.Object.SqlClient.Caching.Debugging;
    using Allors.Adapters.Object.SqlClient.Debug;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using NLog;
    
    public abstract class Scheduler
    {
        protected Scheduler()
        {
            this.Configuration = new ConfigurationBuilder()
                .AddJsonFile(@"appSettings.json")
                .Build();

            this.ObjectFactory = new Allors.ObjectFactory(MetaPopulation.Instance, typeof(User));

            this.Logger = LogManager.GetCurrentClassLogger();
        }
        
        protected IConfigurationRoot Configuration { get; set; }

        protected Allors.ObjectFactory ObjectFactory { get; set; }


        protected DebugConnectionFactory ConnectionFactory { get; private set; }

        protected DebugCacheFactory CacheFactory { get; private set; }

        protected Logger Logger { get; }

        protected string DataPath => this.Configuration["dataPath"];

        protected DirectoryInfo DataDirectoryInfo => new DirectoryInfo(this.DataPath);

        protected string Link => this.Configuration["link"];

        public abstract void Schedule();

        protected IDatabase CreateDatabase(IsolationLevel isolationLevel = IsolationLevel.Snapshot)
        {
            this.ConnectionFactory = new DebugConnectionFactory();
            this.CacheFactory = new DebugCacheFactory();

            var configuration = new Configuration
                                    {
                                        ConnectionString = this.Configuration["allors"],
                                        ObjectFactory = this.ObjectFactory,
                                        IsolationLevel = isolationLevel,
                                        CommandTimeout = 0,
                                        ConnectionFactory = this.ConnectionFactory,
                                        CacheFactory = this.CacheFactory
                                    };

            var services = new ServiceCollection();
            services.AddAllorsEmbedded();
            var serviceProvider = services.BuildServiceProvider();

            var database = new Database(serviceProvider, configuration);

            return database;
        }
    }
}
