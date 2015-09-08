namespace Allors.Meta
{
    public partial class PartRevisionClass
	{
	    internal override void AppsExtend()
        {
            this.Part.RoleType.IsRequired = true;
            this.SupersededByPart.RoleType.IsRequired = true;
        }
	}
}