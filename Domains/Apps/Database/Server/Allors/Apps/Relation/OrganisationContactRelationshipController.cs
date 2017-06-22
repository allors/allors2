namespace Allors.Server.Controllers
{
    using System.Threading.Tasks;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class OrganisationContactRelationshipController : Controller
    {
        private readonly IAllorsContext allors;

        public OrganisationContactRelationshipController(IAllorsContext allorsContext)
        {
            this.allors = allorsContext;
        }

        [HttpPost]
        public IActionResult Pull([FromBody] Model model)
        {
            var response = new PullResponseBuilder(this.allors.User);

            var organisationContactRelationship = (OrganisationContactRelationship)this.allors.Session.Instantiate(model.Id);
            response.AddObject("organisationContactRelationship", organisationContactRelationship);

            response.AddObject("contact", organisationContactRelationship.Contact);

            var locales = new Locales(this.allors.Session).Extent();
            response.AddCollection("locales", locales);

            var genders = new GenderTypes(this.allors.Session).Extent();
            response.AddCollection("genders", genders);

            var salutations = new Salutations(this.allors.Session).Extent();
            response.AddCollection("salutations", salutations);

            var contactKinds = new OrganisationContactKinds(this.allors.Session).Extent();
            response.AddCollection("organisationContactKinds", contactKinds);

            return this.Ok(response.Build());
        }

        public class Model
        {
            public string Id { get; set; }
        }
    }
}