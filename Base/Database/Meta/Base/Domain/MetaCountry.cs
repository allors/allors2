namespace Allors.Meta
{
    public partial class MetaCountry
    {
        internal override void BaseExtend()
        {
            this.IsoCode.IsRequired = true;
            this.IsoCode.IsUnique = true;
            this.Name.IsRequired = true;
        }
    }
}