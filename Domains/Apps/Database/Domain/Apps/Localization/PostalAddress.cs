// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostalAddress.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System.Text;
    using Meta;

    public partial class PostalAddress
    {
        private const string Break = "<br />";

        public bool IsPostalAddress => true;

        public string FullAddress
        {
            get
            {
                if (this.ExistFormattedFullAddress)
                {
                    return this.FormattedFullAddress.Replace(Break, ", ");
                }

                return string.Empty;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.DeriveFormattedfullAddress();
            this.AppsOnDerivePostalCode();
            this.AppsOnDeriveCity();
            this.AppsOnDeriveCountry();

            this.Description = this.FormattedFullAddress;
        }

        private static void AppendNextLine(StringBuilder fullAddress)
        {
            if (fullAddress.Length > 0)
            {
                fullAddress.Append(Break);
            }
        }

        public void AppsOnDerivePostalCode()
        {
            foreach (GeographicBoundary geographicBoundary in this.GeographicBoundaries)
            {
                if (geographicBoundary is PostalCode)
                {
                    this.PostalCode = (PostalCode)geographicBoundary;
                    break;
                }
            }
        }

        public void AppsOnDeriveCity()
        {
            foreach (GeographicBoundary geographicBoundary in this.GeographicBoundaries)
            {
                if (geographicBoundary is City)
                {
                    this.City = (City)geographicBoundary;
                    break;
                }
            }
        }

        public void AppsOnDeriveCountry()
        {
            foreach (GeographicBoundary geographicBoundary in this.GeographicBoundaries)
            {
                if (geographicBoundary is Country)
                {
                    this.Country = (Country)geographicBoundary;
                    break;
                }
            }
        }

        private void DeriveFormattedfullAddress()
        {            
            var fullAddress = new StringBuilder();

            if (!string.IsNullOrEmpty(this.Address1))
            {
                fullAddress.Append(this.Address1);
            }

            if (!string.IsNullOrEmpty(this.Address2))
            {
                AppendNextLine(fullAddress);
                fullAddress.Append(this.Address2);
            }

            if (!string.IsNullOrEmpty(this.Address3))
            {
                AppendNextLine(fullAddress);
                fullAddress.Append(this.Address3);
            }

            if (this.ExistGeographicBoundaries)
            {
                AppendNextLine(fullAddress);

                foreach (GeographicBoundary geographicBoundary in this.GeographicBoundaries)
                {
                    var postalCode = geographicBoundary as PostalCode;
                    if (postalCode != null)
                    {
                        fullAddress.Append(postalCode.Code);
                        fullAddress.Append(" ");
                    }
                }

                foreach (GeographicBoundary geographicBoundary in this.GeographicBoundaries)
                {
                    var city = geographicBoundary as City;
                    if (city != null)
                    {
                        fullAddress.Append(city.Name);
                    }
                }

                foreach (GeographicBoundary geographicBoundary in this.GeographicBoundaries)
                {
                    var country = geographicBoundary as Country;
                    if (country != null)
                    {
                        AppendNextLine(fullAddress);
                        fullAddress.Append(country.Name);
                    }
                }
            }

            this.FormattedFullAddress = fullAddress.ToString();
        }
    }
}