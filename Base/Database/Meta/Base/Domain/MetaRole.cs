namespace Allors.Meta
{
    public partial class MetaRole
    {
        internal override void BaseExtend()
        {
            this.Name.IsRequired = true;
            this.Name.IsUnique = true;
        }
    }
}