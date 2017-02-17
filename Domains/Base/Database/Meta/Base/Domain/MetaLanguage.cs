namespace Allors.Meta
{
    public partial class MetaLanguage
    {
        internal override void BaseExtend()
        {
            this.IsoCode.IsRequired = true;
            this.Name.IsRequired = true;
        }
    }
}