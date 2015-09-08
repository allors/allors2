namespace Allors.Meta
{
    public partial class UniquelyIdentifiableInterface
	{
	    internal override void AppsExtend()
        {
			this.UniqueId.RoleType.IsRequired = true;

		}
	}
}