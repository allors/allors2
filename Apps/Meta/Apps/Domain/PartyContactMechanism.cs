namespace Allors.Meta
{
    public partial class PartyContactMechanismClass
	{
	    internal override void AppsExtend()
        {
            this.ContactMechanism.RoleType.IsRequired = true;
            this.UseAsDefault.RoleType.IsRequired = true;
        }
	}
}