namespace Allors.Meta
{
    public partial class TransferStatusClass
	{
	    internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.TransferObjectState.RoleType.IsRequired = true;
        }
	}
}