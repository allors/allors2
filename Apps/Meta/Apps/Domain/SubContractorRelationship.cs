namespace Allors.Meta
{
    public partial class SubContractorRelationshipClass
	{
	    internal override void AppsExtend()
        {
            this.Contractor.RoleType.IsRequired = true;
            this.SubContractor.RoleType.IsRequired = true;
		}
	}
}