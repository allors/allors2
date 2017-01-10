namespace Allors.Meta
{
    public partial class MetaLocale
    {
        internal override void BaseExtend()
        {
            this.Language.IsRequired = true;
            this.Country.IsRequired = true;
        }
    }
}