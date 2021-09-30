namespace Web.Controllers
{
    using System.Linq;
    using Allors;
    using Allors.Data;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server;
    using Allors.Services;
    using Allors.Workspace.Meta;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class PeopleController : Controller
    {
        private readonly ISessionService allors;
        private readonly ITreeService treeService;

        public PeopleController(ISessionService allors, ITreeService treeService)
        {
            this.allors = allors;
            this.treeService = treeService;
        }

        [Authorize]
        [HttpPost]
        public ActionResult Pull()
        {
            var user = (Person)this.allors.Session.GetUser();
            var responseBuilder = new PullResponseBuilder(user, this.treeService);

            var peopleTree = new Node[]
            {
                new Node(M.Person.Addresses),
            };

            responseBuilder.AddCollection("people", new People(this.allors.Session).Extent(), peopleTree);

            return this.Ok(responseBuilder.Build());
        }
    }
}
