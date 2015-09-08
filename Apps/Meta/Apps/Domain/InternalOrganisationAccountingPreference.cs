namespace Allors.Meta
{
    public partial class InternalOrganisationAccountingPreferenceClass
	{
	    internal override void AppsExtend()
        {
            this.InternalOrganisation.RoleType.IsRequired = true;
        }
	}
}