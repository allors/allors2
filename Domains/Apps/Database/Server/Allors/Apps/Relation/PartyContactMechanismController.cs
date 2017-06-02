
namespace Allors.Server.Controllers
{
    using System.Threading.Tasks;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server;

    using Microsoft.AspNetCore.Mvc;

    public class PartyContactMechanismController : PullController
    {
        public PartyContactMechanismController(IAllorsContext allorsContext): base(allorsContext)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Pull([FromBody] Model model)
        {
            await this.OnInit();

            var response = new PullResponseBuilder(this.AllorsUser);

            var partyContactMechanism = (PartyContactMechanism)this.AllorsSession.Instantiate(model.Id);
            response.AddObject("partyContactMechanism", partyContactMechanism, M.PartyContactMechanism.AngularDetail);

            var contactMechanismPurposes = new ContactMechanismPurposes(this.AllorsSession).Extent();
            response.AddCollection("contactMechanismPurposes", contactMechanismPurposes);

            return this.Ok(response.Build());
        }
    }

    public class Model
    {
        public string Id { get; set; }
    }
}