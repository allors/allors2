namespace Allors.Server
{
    using System;
    using Allors;

    public class AllorsContext : IAllorsContext, IDisposable
    {
        public ISession Session { get; set; }

        public AllorsContext(IDatabase database)
        {
          this.Session = database.CreateSession();
        }

        public void Dispose()
        {
            this.Session.Rollback();
            this.Session = null;
        }
    }
}