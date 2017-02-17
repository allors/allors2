namespace Allors.Meta
{
    public partial class MetaCurrency
    {
        internal override void BaseExtend()
        {
            this.IsoCode.IsRequired = true;
            this.Name.IsRequired = true;
            this.Symbol.IsRequired = true;
        }
    }
}