namespace Allors.Server
{
    using System;

    using Allors.Adapters.Object.SqlClient;

    public class AllorsContext : IAllorsContext, IDisposable
    {
        public ISession Session { get; set; }

        public AllorsContext(IObjectFactory objectFactory)
        {
            var configuration = new Configuration
            {
                ObjectFactory = objectFactory,
                ConnectionString = "server=(local);database=custom;Integrated Security=SSPI"
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