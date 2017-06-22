namespace Allors.Server
{
    using System;
    using Allors.Domain;

    public class AllorsContext : IAllorsContext, IDisposable
    {
        public AllorsContext(IDatabase database, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            this.Session = database.CreateSession();

            var httpContext = httpContextAccessor.HttpContext;
            var userName = httpContext.User.Identity.Name;

            var users = new Users(this.Session);
            this.User = users.GetUser(userName) ?? Singleton.Instance(this.Session).Guest;
            users.CurrentUser = this.User;
        }

        public ISession Session { get; private set; }

        public User User { get; private set;  }

        public void Dispose()
        {
            this.Session.Rollback();
            this.Session = null;
            this.User = null;
        }
    }
}