//------------------------------------------------------------------------------------------------- 
// <copyright file="Place.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Place type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Allors.Meta;

    /// <summary>
    /// A Place is a fysical location somewhere on earth, defined as a postalcode/city and a country
    /// </summary>
    public partial class Place
    {
        public void CustomOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertExists(this, M.Place.PostalCode);
            derivation.Validation.AssertNonEmptyString(this, M.Place.PostalCode);
            derivation.Validation.AssertExists(this, M.Place.City);
            derivation.Validation.AssertNonEmptyString(this, M.Place.City);
        }
    }
}
