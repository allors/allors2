// <copyright file="PostalAddress.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Diagnostics;

namespace Allors.Domain
{
    using Allors.Meta;

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
