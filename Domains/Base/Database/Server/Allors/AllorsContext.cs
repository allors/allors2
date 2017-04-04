using System;

namespace Allors.Web.Database
{
    public class AllorsContext : IAllorsContext, IDisposable
    {
        public ISession Session { get; set; }

        public AllorsContext(IObjectFactory objectFactory)
        {
            var configuration = new Adapters.Object.SqlClient.Configuration{ ObjectFactory = objectFactory};
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