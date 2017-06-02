namespace Allors.Server
{
    using System;

    public class AllorsContext : IAllorsContext, IDisposable
    {
        public AllorsContext(IDatabase database)
        {
            this.Session = database.CreateSession();
        }

        public ISession Session { get; set; }

        public void Dispose()
        {
            this.Session.Rollback();
            this.Session = null;
        }
    }
}