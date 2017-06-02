namespace Allors.Server.Controllers
{
    using System.Threading.Tasks;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class OrganisationContactRelationshipController : PullController
    {
    public OrganisationContactRelationshipController(IAllorsContext allorsContext) : base(allorsContext)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Pull([FromBody] Model model)
    {
        await this.OnInit();

        var response = new PullResponseBuilder(this.AllorsUser);

        var organisationContactRelationship = (OrganisationContactRelationship) this.AllorsSession.Instantiate(model.Id);
        response.AddObject("organisationContactRelationship", organisationContactRelationship);

        response.AddObject("contact", organisationContactRelationship.Contact);

        var locales = new Locales(this.AllorsSession).Extent();
        response.AddCollection("locales", locales);

        var genders = new GenderTypes(this.AllorsSession).Extent();
        response.AddCollection("genders", genders);

        var salutations = new Salutations(this.AllorsSession).Extent();
        response.AddCollection("salutations", salutations);

        var contactKinds = new OrganisationContactKinds(this.AllorsSession).Extent();
        response.AddCollection("organisationContactKinds", contactKinds);

        return this.Ok(response.Build());
    }

    public class Model
    {
        public string Id { get; set; }
    }
    }
}