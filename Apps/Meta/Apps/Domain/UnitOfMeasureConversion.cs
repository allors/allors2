namespace Allors.Meta
{
    public partial class UnitOfMeasureConversionClass
	{
	    internal override void AppsExtend()
        {
            this.ToUnitOfMeasure.RoleType.IsRequired = true;
            this.ConversionFactor.RoleType.IsRequired = true;
        }
	}
}