namespace Allors.Meta
{
    public partial class MetaUniquelyIdentifiable
    {
        internal override void BaseExtend()
        {
            this.UniqueId.IsRequired = true;
        }
    }
}