namespace src.allors.material.apps.objects.emailaddress.edit
{
    using Allors.Meta;
    using Angular.Material;

    public partial class EmailAddressEditComponent
    {
        public MaterialSelect<EmailAddressEditComponent> ContactPurposes => this.MaterialSelect(M.PartyContactMechanism.ContactPurposes);
    }
}
