namespace Web.Controllers
{
    using System.Web.Mvc;

    using Allors;
    using Allors.Web.Database;

    public abstract class PullController : CustomController
    {
        protected JsonResult Derive()
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

            return this.JsonSuccess(invokeResponse);
        }
    }
}