namespace Allors.Meta
{
    public partial class NeededSkillClass
	{
	    internal override void AppsExtend()
        {
            this.Skill.RoleType.IsRequired = true;
            this.SkillLevel.RoleType.IsRequired = true;
        }
	}
}