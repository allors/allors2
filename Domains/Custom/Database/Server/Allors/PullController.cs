using Allors;
using Allors.Domain;
using Allors.Meta;
using Microsoft.AspNetCore.Mvc;

namespace Allors.Server.Controllers
{
    public abstract class PullController : Controller
    {
        public ISession AllorsSession { get; set; }

        public User AllorsUser { get; set; }

        protected PullController(IAllorsContext allorsContext)
        {
            this.AllorsSession = allorsContext.Session;
            this.AllorsUser = new Users(this.AllorsSession).FindBy(M.User.UserName, "administrator");
        }

        protected IActionResult Derive()
        {
            var invokeResponse = new InvokeResponse();

            var validation = this.AllorsSession.Derive();
            if (validation.HasErrors)
            {
                invokeResponse.AddDerivationErrors(validation);
            }
            else
            {
                this.AllorsSession.Commit();
            }

            return this.Ok(invokeResponse);
        }
    }
}
