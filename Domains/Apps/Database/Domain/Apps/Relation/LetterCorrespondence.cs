// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LetterCorrespondence.cs" company="Allors bvba">
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

using System;

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class LetterCorrespondence
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.LetterCorrespondence, M.LetterCorrespondence.CommunicationEventState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void AppsOnDerive(ObjectOnDerive method)
        {
            this.AppsOnDeriveFromParties();
            this.AppsOnDeriveToParties();
            this.AppsOnDeriveInvolvedParties();

            if (this.Originators.Count == 0 || this.Receivers.Count == 0)
            {
                this.Delete();
            }
        }

        public void AppsOnDeriveFromParties()
        {
            this.RemoveFromParties();
            this.FromParties = this.Originators;
        }

        public void AppsOnDeriveToParties()
        {
            this.RemoveToParties();
            this.ToParties = this.Receivers;
        }

        // TODO: Can we move this to shared interface?
        public void AppsOnDeriveInvolvedParties()
        {
            this.RemoveInvolvedParties();
            this.InvolvedParties = this.Receivers;

            foreach (Party originator in this.Originators)
            {
                this.AddInvolvedParty(originator);
            }

            if (this.ExistOwner && !this.InvolvedParties.Contains(this.Owner))
            {
                this.AddInvolvedParty(this.Owner);
            }

            foreach (Party party in this.PartiesWhereCommunicationEvent)
            {
                this.AddInvolvedParty(party);
            }

            foreach (Party party in this.InvolvedParties)
            {
                if (party is Person person)
                {
                    foreach (OrganisationContactRelationship organisationContactRelationship in person.OrganisationContactRelationshipsWhereContact)
                    {
                        if (organisationContactRelationship.FromDate <= DateTime.UtcNow &&
                            (!organisationContactRelationship.ExistThroughDate || organisationContactRelationship.ThroughDate >= DateTime.UtcNow))
                        {
                            this.AddInvolvedParty(organisationContactRelationship.Organisation);
                        }
                    }
                }
            }
        }
    }
}
