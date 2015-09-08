namespace Allors.Meta
{
    public partial class ShipmentPackageClass
	{
	    internal override void AppsExtend()
        {
			this.SequenceNumber.RoleType.IsRequired = true;

            this.CreationDate.RoleType.IsRequired = true;
		}
	}
}