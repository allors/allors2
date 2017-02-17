namespace Allors.Meta
{
    public partial class MetaCounter
    {
        internal override void BaseExtend()
        {
            this.Value.IsRequired = true;
        }
    }
}