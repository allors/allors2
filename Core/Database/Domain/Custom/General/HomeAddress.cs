//------------------------------------------------------------------------------------------------- 
// <copyright file="HomeAddress.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the HomeAddress type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Allors.Meta;

    /// <summary>
    /// A HomeAddress is a fysical address with a street/number and a place
    /// </summary>
    public partial class HomeAddress
    {
        public void CustomOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertExists(this, M.HomeAddress.Street);
            derivation.Validation.AssertNonEmptyString(this, M.HomeAddress.Street);
        }
    }
}
