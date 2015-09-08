namespace Allors.Meta
{
    public partial class ResumeClass
	{
	    internal override void AppsExtend()
        {
            this.ResumeDate.RoleType.IsRequired = true;
            this.ResumeText.RoleType.IsRequired = true;
        }
	}
}