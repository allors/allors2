namespace Web.Controllers
{
using System.Web.Mvc;

using Allors.Meta;
using Allors.Web.Database;

    public class ProfileController : PullController
    {
        private static Tree tree;

        private static Tree Tree => tree ?? (tree = new Tree(M.Person)
            .Add(M.Person.Photo));

        [Authorize]
        [HttpPost]
        public ActionResult Pull()
        {
            var responseBuilder = new PullResponseBuilder(this.AllorsUser);
            responseBuilder.AddObject("person", this.AllorsUser, Tree);
            return this.JsonSuccess(responseBuilder.Build());
        }
    }
}