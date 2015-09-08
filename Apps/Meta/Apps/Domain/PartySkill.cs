namespace Allors.Meta
{
    public partial class PartySkillClass
	{
	    internal override void AppsExtend()
        {
            this.Skill.RoleType.IsRequired = true;
        }
	}
}