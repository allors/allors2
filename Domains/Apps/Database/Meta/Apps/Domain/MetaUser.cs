namespace Allors.Meta
{
    public partial class MetaUser
    {
        internal override void AppsExtend()
        {
            this.UserEmail.RelationType.Workspace = true;
            this.UserName.RelationType.Workspace = true;
        }
    }
}