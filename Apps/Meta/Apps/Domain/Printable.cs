namespace Allors.Meta
{
    public partial class PrintableInterface
	{
	    internal override void AppsExtend()
        {
			this.PrintContent.RoleType.IsRequired = true;

		}
	}
}