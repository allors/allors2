namespace Allors.Meta
{
    public partial class MediaContentClass
	{
	    internal override void AppsExtend()
        {
			this.Value.RoleType.IsRequired = true;
			this.Hash.RoleType.IsRequired = true;

		}
	}
}