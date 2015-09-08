namespace Allors.Meta
{
    public partial class RespondingPartyClass
	{
	    internal override void AppsExtend()
        {
            this.Party.RoleType.IsRequired = true;
        }
	}
}