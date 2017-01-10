namespace Allors.Meta
{
    public partial class MetaUserGroup
    {
        internal override void BaseExtend()
        {
            this.Name.IsRequired = true;
            this.Name.IsUnique = true;
        }
    }
}