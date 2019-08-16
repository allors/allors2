
// <copyright file="PersonalTitles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class PersonalTitles
    {
        private UniquelyIdentifiableSticky<PersonalTitle> cache;

        private UniquelyIdentifiableSticky<PersonalTitle> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<PersonalTitle>(this.Session));
    }
}
