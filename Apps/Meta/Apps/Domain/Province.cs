namespace Allors.Meta
{
    public partial class ProvinceClass
	{
	    internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
	}
}