namespace Allors
{
    using System.Data;

    using Allors.Adapters.Object.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services.Base;

    using Microsoft.Extensions.Configuration;

    public abstract class Command
    {
        protected Command()
        {
            this.Configuration = new ConfigurationBuilder()
                .AddJsonFile(@"appSettings.json")
                .Build();
            this.ObjectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User));
        }

        protected ObjectFactory ObjectFactory { get; }

        protected IConfigurationRoot Configuration { get; }

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

            var database = new Database(configuration);

            var timeService = new TimeService();
            var mailService = new MailService { DefaultSender = "noreply@example.com" };
            var securityService = new SecurityService();
            var serviceLocator = new ServiceLocator
                                     {
                                         TimeServiceFactory = () => timeService,
                                         MailServiceFactory = () => mailService,
                                         SecurityServiceFactory = () => securityService
                                     };
            database.SetServiceLocator(serviceLocator.Assert());


            return database;
        }
    }
}
