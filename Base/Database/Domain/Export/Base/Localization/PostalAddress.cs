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

using System.Diagnostics;

namespace Allors.Domain
{
    using Meta;

    public partial class PostalAddress
    {
        public bool IsPostalAddress => true;

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertExistsAtMostOne(this, M.PostalAddress.PostalAddressBoundaries, M.PostalAddress.Locality);
            derivation.Validation.AssertExistsAtMostOne(this, M.PostalAddress.PostalAddressBoundaries, M.PostalAddress.Region);
            derivation.Validation.AssertExistsAtMostOne(this, M.PostalAddress.PostalAddressBoundaries, M.PostalAddress.PostalCode);
            derivation.Validation.AssertExistsAtMostOne(this, M.PostalAddress.PostalAddressBoundaries, M.PostalAddress.Country);

            if (!this.ExistPostalAddressBoundaries)
            {
                if (!this.ExistCountry || !this.ExistLocality)
                {
                    Debugger.Break();
                }

                derivation.Validation.AssertExists(this, M.PostalAddress.Locality);
                derivation.Validation.AssertExists(this, M.PostalAddress.Country);
            }
        }
    }
}