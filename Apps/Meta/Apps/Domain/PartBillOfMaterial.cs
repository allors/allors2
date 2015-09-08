namespace Allors.Meta
{
    public partial class PartBillOfMaterialInterface
	{
	    internal override void AppsExtend()
        {
            this.QuantityUsed.RoleType.IsRequired = true;
            this.ComponentPart.RoleType.IsRequired = true;
            this.Part.RoleType.IsRequired = true;
        }
	}
}