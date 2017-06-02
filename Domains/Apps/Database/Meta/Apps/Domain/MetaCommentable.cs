namespace Allors.Meta
{
    public partial class MetaCommentable
    {
        internal override void AppsExtend()
        {
            this.Comment.RelationType.Workspace = true;
        }
    }
}