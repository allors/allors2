namespace Allors.Meta
{
    public partial class ServiceTerritoryClass
	{
	    internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
	}
}