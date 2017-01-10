namespace Allors.Meta
{
    public partial class MetaAccessControl
    {
        internal override void BaseExtend()
        {
            this.Role.IsRequired = true;
        }
    }
}