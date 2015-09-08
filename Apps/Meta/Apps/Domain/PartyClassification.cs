namespace Allors.Meta
{
    public partial class PartyClassificationInterface
	{
        internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
	}
}