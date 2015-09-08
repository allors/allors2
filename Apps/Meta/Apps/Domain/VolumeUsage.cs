namespace Allors.Meta
{
    public partial class VolumeUsageClass
	{
	    internal override void AppsExtend()
	    {
	        this.Quantity.RoleType.IsRequired = true;
            this.UnitOfMeasure.RoleType.IsRequired = true;
        }
	}
}