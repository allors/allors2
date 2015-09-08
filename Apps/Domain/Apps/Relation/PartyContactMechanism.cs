// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyContactMechanism.cs" company="Allors bvba">
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

using System;

namespace Allors.Domain
{
    public partial class PartyContactMechanism
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistFromDate)
            {
                this.FromDate = DateTime.UtcNow;
            }

            if (!this.ExistUseAsDefault)
            {
                this.UseAsDefault = false;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistPartyWherePartyContactMechanism)
            {
                derivation.AddDependency(this.PartyWherePartyContactMechanism, this);
            }
        }


        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistUseAsDefault && this.UseAsDefault)
            {
                derivation.Log.AssertExists(this, PartyContactMechanisms.Meta.ContactPurpose);
            }

            if (this.UseAsDefault && this.ExistPartyWherePartyContactMechanism && this.ExistContactPurpose)
            {
                var partyContactMechanisms = this.PartyWherePartyContactMechanism.PartyContactMechanisms;
                partyContactMechanisms.Filter.AddEquals(PartyContactMechanisms.Meta.ContactPurpose, this.ContactPurpose);

                foreach (PartyContactMechanism partyContactMechanism in partyContactMechanisms)
                {
                    if (!partyContactMechanism.Equals(this))
                    {
                        partyContactMechanism.UseAsDefault = false;
                    }
                }   
            }
        }
        public bool IsActive
        {
            get { return this.ExistPartyWhereCurrentPartyContactMechanism; }
        }
    }
}