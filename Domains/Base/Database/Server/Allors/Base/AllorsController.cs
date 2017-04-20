namespace Allors.Server
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Allors.Domain;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(ActiveAuthenticationSchemes = AuthenticationController.Scheme)]
    public abstract class AllorsController : Controller
    {
        protected AllorsController(IAllorsContext allorsContext)
        {
            this.AllorsSession = allorsContext.Session;
        }
        
        public ISession AllorsSession { get; }

        public User AllorsUser { get; private set; }

        protected async Task OnInit()
        {
            await this.HttpContext.Authentication.GetAuthenticateInfoAsync(AuthenticationController.Scheme);
            var claim = this.HttpContext.User.Claims.FirstOrDefault(v => ClaimTypes.NameIdentifier.Equals(v.Type));
            if (claim != null)
            {
                this.AllorsUser = (User)this.AllorsSession.Instantiate(claim.Value);
            }
        }
    }
}