namespace Web.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;

    using Allors.Web.Security;

    using Mvc.Models;

    [Authorize]
    public class AccountController : CustomController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var membershipProvider = new AllorsMembershipProvider();
            if (!membershipProvider.ValidateUser(model.Email, model.Password))
            {
                this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return this.View(model);
            }

            FormsAuthentication.SetAuthCookie(model.Email, false);
            return this.RedirectToLocal(returnUrl);
        }

        [Authorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return this.RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }
    }
}