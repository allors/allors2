// <copyright file="Organisation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Person type.</summary>

namespace Allors.Domain
{
    public partial class Organisations
    {
        private UniquelyIdentifiableSticky<Organisation> cache;

        public UniquelyIdentifiableSticky<Organisation> Cache => this.cache ??= new UniquelyIdentifiableSticky<Organisation>(this.Session);
    }
}
