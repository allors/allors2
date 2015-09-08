// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostalAddresses.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class PostalAddresses
    {
        public PostalAddress GetPostalAddress(string address1, string address2, string address3, PostalCode postalCode, City city, Country country)
        {
            var postalAddresses = new PostalAddresses(this.Session).Extent();

            if (!string.IsNullOrEmpty(address1))
            {
                postalAddresses.Filter.AddEquals(Meta.Address1, address1);
            }

            if (!string.IsNullOrEmpty(address2))
            {
                postalAddresses.Filter.AddEquals(Meta.Address2, address2);
            }

            if (!string.IsNullOrEmpty(address3))
            {
                postalAddresses.Filter.AddEquals(Meta.Address3, address3);
            }

            if (postalCode != null)
            {
                postalAddresses.Filter.AddContains(Meta.GeographicBoundaries, postalCode);
            }

            if (city != null)
            {
                postalAddresses.Filter.AddContains(Meta.GeographicBoundaries, city);
            }

            if (country != null)
            {
                postalAddresses.Filter.AddContains(Meta.GeographicBoundaries, country);
            }

            return postalAddresses.First;
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}