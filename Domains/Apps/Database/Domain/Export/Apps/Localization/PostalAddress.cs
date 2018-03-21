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
        public bool IsPostalAddress => true;

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, M.PostalAddress.GeographicBoundaries, M.PostalAddress.PostalBoundary);
            derivation.Validation.AssertExistsAtMostOne(this, M.PostalAddress.GeographicBoundaries, M.PostalAddress.PostalBoundary);

            this.AppsOnDerivePostalCode();
            this.AppsOnDeriveCity();
            this.AppsOnDeriveCountry();
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

            if (this.ExistPostalBoundary)
            {
                this.Country = this.PostalBoundary.Country;
            }
        }
    }
}