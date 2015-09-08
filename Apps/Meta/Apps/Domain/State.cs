namespace Allors.Meta
{
    public partial class StateClass
	{
	    internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
	}
}