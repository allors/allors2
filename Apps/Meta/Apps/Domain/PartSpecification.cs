namespace Allors.Meta
{
    public partial class PartSpecificationInterface
	{
	    internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
            this.CurrentObjectState.RoleType.IsRequired = true;
        }
	}
}