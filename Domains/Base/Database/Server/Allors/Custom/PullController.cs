namespace Allors.Server.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public abstract class PullController : AllorsController
    {
        protected PullController(IAllorsContext allorsContext) : base(allorsContext)
        {
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
