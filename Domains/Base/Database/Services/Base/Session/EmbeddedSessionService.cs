namespace Allors.Services
{
    using System;

    using Allors.Domain;

    using Microsoft.AspNetCore.Http;

    public partial class EmbeddedSessionService : ISessionService, IDisposable
    {
        public EmbeddedSessionService(IDatabaseService databaseService, IHttpContextAccessor httpContextAccessor)
        {
            this.Session = databaseService.Database.CreateSession();

            var userName = httpContextAccessor.HttpContext.User.Identity.Name;
            var users = new Users(this.Session);
            var user = users.GetUser(userName) ?? this.Session.GetSingleton()?.Guest;

            this.Session.SetUser(user);
        }

        public Allors.ISession Session { get; private set; }

        public void Dispose()
        {
            this.Session.Rollback();
            this.Session = null;
        }
    }
}