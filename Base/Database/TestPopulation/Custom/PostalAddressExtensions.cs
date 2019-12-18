namespace Allors.Domain.TestPopulation
{
    using Allors.Domain;
 
    public static partial class PostalAddressExtensions
    {
        public static string DisplayName(this PostalAddress @this)
        {
            var fullAddress = string.Empty;
            if (@this.ExistAddress1 || @this.ExistAddress2 || @this.ExistAddress3)
            {
                if (@this.ExistAddress1)
                {
                    fullAddress = @this.Address1;
                }
                else if (@this.ExistAddress2)
                {
                    fullAddress = @this.Address2;
                }
                else
                {
                    fullAddress = @this.Address3;
                }
            }

            if (string.IsNullOrEmpty(fullAddress))
            {
                fullAddress += @this.PostalCode;
            }
            else
            {
                fullAddress += ' ' + @this.PostalCode;
            }

            if (string.IsNullOrEmpty(fullAddress))
            {
                fullAddress += @this.Locality;
            }
            else
            {
                fullAddress += ' ' + @this.Locality;
            }

            if (string.IsNullOrEmpty(fullAddress))
            {
                return fullAddress += @this.Country.Name;
            }
            else
            {
                return fullAddress += ' ' + @this.Country.Name;
            }
        }
    }
}
