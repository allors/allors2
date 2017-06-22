namespace Allors.Console
{
    using System.Data;
    using System.Security.Claims;
    using System.Security.Principal;

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

        protected IConfigurationRoot Configuration { get; }

        protected ObjectFactory ObjectFactory { get; }

        protected string DataPath => this.Configuration["dataPath"];

        public abstract void Execute();
        
        protected void SetIdentity(string identity)
        {
            ClaimsPrincipal.ClaimsPrincipalSelector = () => new GenericPrincipal(new GenericIdentity(identity, "Forms"), new string[0]);
        }

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
            var mailService = new MailService();
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
