namespace Allors.Server
{
    using System;
    using Allors;

    public class AllorsContext : IAllorsContext, IDisposable
    {
        public ISession Session { get; set; }

        public AllorsContext(IObjectFactory objectFactory)
        {
            var configuration = new Allors.Adapters.Object.SqlClient.Configuration{ ObjectFactory = objectFactory};
            var database = new Allors.Adapters.Object.SqlClient.Database(configuration);
            this.Session = database.CreateSession();
        }

        public void Dispose()
        {
            this.Session.Rollback();
            this.Session = null;
        }
    }
}