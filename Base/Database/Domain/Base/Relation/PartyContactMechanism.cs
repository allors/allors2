// <copyright file="PartyContactMechanism.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class PartyContactMechanism
    {
        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistFromDate)
            {
                this.FromDate = this.Session().Now();
            }

            if (!this.ExistUseAsDefault)
            {
                this.UseAsDefault = false;
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                if (this.ExistPartyWherePartyContactMechanism)
                {
                    iteration.AddDependency(this.PartyWherePartyContactMechanism, this);
                    iteration.Mark(this.PartyWherePartyContactMechanism);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistUseAsDefault && this.UseAsDefault)
            {
                derivation.Validation.AssertExists(this, M.PartyContactMechanism.ContactPurposes);
            }

            if (this.UseAsDefault && this.ExistPartyWherePartyContactMechanism && this.ExistContactPurposes)
            {
                foreach (var contactMechanismPurpose in this.ContactPurposes)
                {
                    var partyContactMechanisms = this.PartyWherePartyContactMechanism.PartyContactMechanisms;
                    partyContactMechanisms.Filter.AddContains(M.PartyContactMechanism.ContactPurposes, (IObject)contactMechanismPurpose);

                    foreach (PartyContactMechanism partyContactMechanism in partyContactMechanisms)
                    {
                        if (!partyContactMechanism.Equals(this))
                        {
                            partyContactMechanism.UseAsDefault = false;
                        }
                    }
                }
            }
        }
    }
}
