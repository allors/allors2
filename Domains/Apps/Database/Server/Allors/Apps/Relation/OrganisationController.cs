namespace Allors.Server.Controllers
{
    using System.Threading.Tasks;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class OrganisationController : AllorsController
    {
        public OrganisationController(IAllorsContext allorsContext): base(allorsContext)
        {
        }
        
        [HttpPost]
        public async Task<IActionResult> Pull([FromBody] Model model)
        {
            await this.OnInit();

            var response = new PullResponseBuilder(this.AllorsUser);

            var organisation = (Party)this.AllorsSession.Instantiate(model.Id);
            response.AddObject("organisation", organisation, M.Organisation.AngularDetail);

            var internalOrganisation = Singleton.Instance(this.AllorsSession).DefaultInternalOrganisation;
            response.AddObject("internalOrganisation", internalOrganisation);

            var locales = new Locales(this.AllorsSession).Extent();
            response.AddCollection("locales", locales);

            var countries = new Countries(this.AllorsSession).Extent();
            response.AddCollection("countries", countries);

            var genders = new GenderTypes(this.AllorsSession).Extent();
            response.AddCollection("genders", genders);

            var salutations = new Salutations(this.AllorsSession).Extent();
            response.AddCollection("salutations", salutations);

            var contactKinds = new OrganisationContactKinds(this.AllorsSession).Extent();
            response.AddCollection("organisationContactKinds", contactKinds);

            var contactMechanismPurposes = new ContactMechanismPurposes(this.AllorsSession).Extent();
            response.AddCollection("contactMechanismPurposes", contactMechanismPurposes);

            response.AddCollection("communicationEvents", organisation?.CommunicationEventsWhereInvolvedParty);

            return this.Ok(response.Build());
        }

        public class Model
        {
            public string Id { get; set; }
        }
    }
}