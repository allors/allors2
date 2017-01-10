namespace Web.Controllers
{
    using System;
    using System.Web.Mvc;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Web.Database;

    public class EditOrganisationController : PullController
    {
        private static Tree tree;
        private static Tree peopleTree;

        private static Tree Tree => tree ?? (tree = new Tree(M.Organisation)
            .Add(M.Organisation.Owner)
            .Add(M.Organisation.Employees)
            .Add(M.Organisation.CycleOne, PeopleTree)
            .Add(M.Organisation.CycleMany, PeopleTree));

        private static Tree PeopleTree => peopleTree ?? (tree = new Tree(M.Person)
                .Add(M.Person.CycleOne)
                .Add(M.Person.CycleMany));

        [Authorize]
        [HttpPost]
        public ActionResult Pull(string id)
        {
            var response = new PullResponseBuilder(this.AllorsUser);

            var organisation = this.AllorsSession.Instantiate(id);
            response.AddObject("organisation", organisation, Tree);

            response.AddCollection("people", new People(this.AllorsSession).Extent(), PeopleTree);

            return this.JsonSuccess(response.Build());
        }
    }
}