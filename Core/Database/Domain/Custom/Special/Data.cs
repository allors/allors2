// <copyright file="AccessClass.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Data
    {
        public void CustomOnDerive(ObjectOnDerive method)
        {
            var singleton = this.Strategy.Session.GetSingleton();

            this.AutocompleteDerivedFilter = this.AutocompleteAssignedFilter ?? singleton.AutocompleteDefault;
            this.AutocompleteDerivedOptions = this.AutocompleteAssignedOptions ?? singleton.AutocompleteDefault;
        }
    }
}
