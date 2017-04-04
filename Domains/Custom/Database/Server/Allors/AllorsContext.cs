namespace Allors.Web.Database
{
    using System;

    public class AllorsContext : IAllorsContext, IDisposable
    {
        public ISession Session { get; set; }

        public AllorsContext(IObjectFactory objectFactory)
        {
            var configuration = new Adapters.Object.SqlClient.Configuration
            {
                ObjectFactory = objectFactory,
                ConnectionString = "server=(local);database=custom;Integrated Security=SSPI"
            };

            var database = new Adapters.Object.SqlClient.Database(configuration);
            this.Session = database.CreateSession();
        }

        public void Dispose()
        {
            this.Session.Rollback();
            this.Session = null;
        }
    }
}