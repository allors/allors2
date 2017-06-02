namespace Allors.Meta
{
    public partial class MetaAuditable
    {
        internal override void AppsExtend()
        {
            this.CreatedBy.RelationType.Workspace = true;
            this.CreationDate.RelationType.Workspace = true;
            this.LastModifiedBy.RelationType.Workspace = true;
            this.LastModifiedDate.RelationType.Workspace = true;
        }
    }
}