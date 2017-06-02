namespace Allors.Server
{
    using System.Linq;
    using System.Security.Claims;

    using Allors;
    using Allors.Domain;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public abstract class AllorsController : Controller
    {
        protected AllorsController(IAllorsContext allorsContext)
        {
            this.AllorsSession = allorsContext.Session;
        }
        
        public ISession AllorsSession { get; }

        public User AllorsUser { get; private set; }

        protected async System.Threading.Tasks.Task OnInit()
        {
            await this.HttpContext.Authentication.GetAuthenticateInfoAsync(JwtBearerDefaults.AuthenticationScheme‌​);
            var claim = this.HttpContext.User.Claims.FirstOrDefault(v => ClaimTypes.NameIdentifier.Equals(v.Type));
            if (claim != null)
            {
                this.AllorsUser = (User)this.AllorsSession.Instantiate(claim.Value);
            }
        }
    }
}