namespace Allors.Meta
{
    public partial class TrainingClass
	{
	    internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
	}
}