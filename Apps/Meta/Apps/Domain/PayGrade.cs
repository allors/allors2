namespace Allors.Meta
{
    public partial class PayGradeClass
	{
	    internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
	}
}