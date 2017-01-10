namespace Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Web.Database;

    public class PersonTypeAheadController : PullController
    {
        [Authorize]
        [HttpPost]
        public ActionResult Pull(string criteria)
        {
            var response = new PullResponseBuilder(this.AllorsUser);

            var people = new People(this.AllorsSession).Extent();
            var or = people.Filter.AddOr();
            or.AddLike(M.Person.LastName, criteria + "%");
            or.AddLike(M.Person.FirstName, criteria + "%");

            response.AddCollection("results", people.Take(100));

            return this.JsonSuccess(response.Build());
        }
    }
}