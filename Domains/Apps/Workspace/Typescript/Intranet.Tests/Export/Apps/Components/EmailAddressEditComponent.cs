using Allors.Meta;
using Components;

namespace src.allors.material.apps.objects.emailaddress.edit
{

    public partial class EmailAddressEditComponent
    {
        public MatSelect<EmailAddressEditComponent> ContactPurposes => this.MatSelect(M.PartyContactMechanism.ContactPurposes);

    }
}
