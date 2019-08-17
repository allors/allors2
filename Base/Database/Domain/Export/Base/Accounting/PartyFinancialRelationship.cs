// <copyright file="PartyFinancialRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Linq;

namespace Allors.Domain
{
    using System;
    using Allors.Meta;

    public partial class PartyFinancialRelationship
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistInternalOrganisation && internalOrganisations.Count() == 1)
            {
                this.InternalOrganisation = internalOrganisations.First();
            }

            this.Parties = new Party[] { this.Party, this.InternalOrganisation };
        }
    }
}
