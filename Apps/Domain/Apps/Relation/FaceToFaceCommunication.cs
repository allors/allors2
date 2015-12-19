// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaceToFaceCommunication.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using Allors.Meta;
    
    public partial class FaceToFaceCommunication
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.FromParties = this.Participants;
            this.ToParties = this.Participants;
            this.AppsOnDeriveInvolvedParties(derivation);

            if (this.Participants.Count <= 1)
            {
                this.Delete();
            }
        }

        public void AppsOnDeriveInvolvedParties(IDerivation derivation)
        {
            this.InvolvedParties = this.Participants;
            this.AddInvolvedParty(this.Owner);
            foreach (Party party in this.PartyRelationshipWhereCommunicationEvent.Parties)
            {
                this.AddInvolvedParty(party);
            }
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            this.RemoveSecurityTokens();
            this.AddSecurityToken(Singleton.Instance(this.Strategy.Session).DefaultSecurityToken);

            if (this.ExistOwner)
            {
                this.AddSecurityToken(Owner.OwnerSecurityToken);
            }

            foreach (Party participant in this.Participants)
            {
                if (participant.GetType().Name == PersonClass.Instance.Name)
                {
                    var person = participant as Person;
                    if (person.ExistCurrentEmployment)
                    {
                        this.AddSecurityToken(person.CurrentEmployment.Employer.OwnerSecurityToken);
                    }

                    if (person.ExistOrganisationContactRelationshipsWhereContact)
                    {
                        foreach (
                            OrganisationContactRelationship organisationContactRelationship in
                                person.OrganisationContactRelationshipsWhereContact)
                        {
                            if (organisationContactRelationship.ExistOrganisation)
                            {
                                foreach (
                                    CustomerRelationship customerRelationship in
                                        organisationContactRelationship.Organisation.CustomerRelationshipsWhereCustomer)
                                {
                                    this.AddSecurityToken(customerRelationship.InternalOrganisation.OwnerSecurityToken);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
