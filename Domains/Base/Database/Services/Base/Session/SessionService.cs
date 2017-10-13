namespace Allors.Services
{
    using System;

    public partial class SessionService : ISessionService, IDisposable
    {
        public SessionService(IDatabaseService databaseService)
        {
            this.Session = databaseService.Database.CreateSession();
        }

        public Allors.ISession Session { get; private set; }

        public void Dispose()
        {
            this.Session.Rollback();
            this.Session = null;
        }
    }
}