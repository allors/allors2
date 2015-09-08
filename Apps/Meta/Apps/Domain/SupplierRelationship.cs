namespace Allors.Meta
{
    public partial class SupplierRelationshipClass
	{
	    internal override void AppsExtend()
        {
            this.Supplier.RoleType.IsRequired = true;
            this.InternalOrganisation.RoleType.IsRequired = true;
            this.SubAccountNumber.RoleType.IsRequired = true;
        }
	}
}