namespace Allors.Meta
{
    public partial class PurchaseReturnStatusClass
	{
	    internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.PurchaseReturnObjectState.RoleType.IsRequired = true;
        }
	}
}