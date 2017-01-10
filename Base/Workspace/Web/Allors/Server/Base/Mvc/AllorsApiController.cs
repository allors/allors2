namespace Allors.Web.Mvc
{
    using System.Web.Http;

    using Allors;
    using Allors.Domain;
    using Allors.Domain.NonLogging;
    using Allors.Meta;

    public class AllorsApiController : ApiController
    {
        private User authenticatedUser;

        protected AllorsApiController()
        {
            this.AllorsSession = Config.Default.CreateSession();
        }

        ~AllorsApiController()
        {
            this.DisposeAllorsSession();
        }

        protected ISession AllorsSession { get; private set; }

        protected virtual User AuthenticatedUser
        {
            get
            {
                if (this.authenticatedUser == null)
                {
                    var userName = this.User.Identity.Name;

                    if (userName != null)
                    {
                        this.authenticatedUser = new Users(this.AllorsSession).FindBy(M.User.UserName, userName);
                    }
                }

                return this.authenticatedUser;
            }

            set { this.authenticatedUser = value; }
            
        }

        protected void AddModelErrors(Validation validation)
        {
            foreach (var error in validation.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisposeAllorsSession();
            }

            base.Dispose(disposing);
        }

        private void DisposeAllorsSession()
        {
            var session = this.AllorsSession;
            if (session != null)
            {
                try
                {
                    session.Dispose();
                }
                finally
                {
                    this.AllorsSession = null;
                }
            }
        }
    }
}
