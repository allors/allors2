namespace Allors.Server
{
    using System;

    using Allors.Adapters.Object.SqlClient;

    using Microsoft.Extensions.Configuration;

    public class AllorsContext : IAllorsContext, IDisposable
    {
        public ISession Session { get; set; }

        public AllorsContext(IObjectFactory objectFactory, IConfigurationRoot configurationRoot)
        {
            var configuration = new Configuration
                                    {
                                        ObjectFactory = objectFactory,
                                        ConnectionString = configurationRoot.GetConnectionString("DefaultConnection")
                                    };

            var database = new Database(configuration);
            this.Session = database.CreateSession();
        }

        public void Dispose()
        {
            this.Session.Rollback();
            this.Session = null;
        }
    }
}