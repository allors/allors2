namespace Allors.Meta
{
    public partial class TerritoryClass
	{
	    internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
	}
}