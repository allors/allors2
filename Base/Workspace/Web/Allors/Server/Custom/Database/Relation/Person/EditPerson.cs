namespace Web.Controllers
{
    using System.Web.Mvc;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Web.Database;

    public class EditPersonController : PullController
    {
        private static Tree tree;

        private static Tree Tree => tree ?? (tree = new Tree(M.Person)
            .Add(M.Person.Photo)
            .Add(M.Person.CycleOne, new Tree(M.Organisation)
                .Add(M.Organisation.CycleOne)
                .Add(M.Organisation.CycleMany))
            .Add(M.Person.CycleMany, new Tree(M.Person)
                .Add(M.Organisation.CycleOne)
                .Add(M.Organisation.CycleMany)));

        [Authorize]
        [HttpPost]
        public ActionResult Pull(string id)
        {
            var response = new PullResponseBuilder(this.AllorsUser);

            response.AddCollection("persons", new People(this.AllorsSession).Extent(), tree);

            var person = this.AllorsSession.Instantiate(id);
            response.AddObject("person", person, tree);

            return this.JsonSuccess(response.Build());
        }
    }
}