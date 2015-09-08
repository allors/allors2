// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneCommunication.cs" company="Allors bvba">
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

using Allors.Meta;

namespace Allors.Domain
{
    public partial class PhoneCommunication
    {
        ObjectState Transitional.CurrentObjectState
        {
            get
            {
                return this.CurrentObjectState;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            this.AppsOnDeriveFromParties();
            this.AppsOnDeriveToParties();
            this.AppsOnDeriveInvolvedParties();
        }

        public void AppsOnDeriveFromParties()
        {
            this.RemoveFromParties();
            this.FromParties = (Extent) this.Callers;

            if (this.IncomingCall)
            {
                var party = this.GetRelationshipWithParty();
                if (party != null)
                {
                    this.AddFromParty(party);
                }
            }
        }

        public void AppsOnDeriveToParties()
        {
            this.RemoveToParties();
            this.ToParties = (Extent)this.Receivers;

            if (!this.IncomingCall)
            {
                var party = this.GetRelationshipWithParty();
                if (party != null)
                {
                    this.AddToParty(party);
                }
            }
        }

        private Party GetRelationshipWithParty()
        {
            var partyRelationship = this.PartyRelationshipWhereCommunicationEvent;
            if (partyRelationship != null)
            {
                if (Equals(this.PartyRelationshipWhereCommunicationEvent.GetType().Name,
                    ClientRelationshipClass.Instance.Name))
                {
                    var relationship = (ClientRelationship) partyRelationship;
                    return relationship.Client;
                }
                if (Equals(this.PartyRelationshipWhereCommunicationEvent.GetType().Name,
                    CustomerRelationshipClass.Instance.Name))
                {
                    var relationship = (CustomerRelationship) partyRelationship;
                    return relationship.Customer;
                }
                if (Equals(this.PartyRelationshipWhereCommunicationEvent.GetType().Name,
                    DistributionChannelRelationshipClass.Instance.Name))
                {
                    var relationship = (DistributionChannelRelationship) partyRelationship;
                    return relationship.Distributor;
                }
                if (Equals(this.PartyRelationshipWhereCommunicationEvent.GetType().Name, EmploymentClass.Instance.Name))
                {
                    var relationship = (Employment) partyRelationship;
                    return relationship.Employee;
                }
                if (Equals(this.PartyRelationshipWhereCommunicationEvent.GetType().Name,
                    OrganisationContactRelationshipClass.Instance.Name))
                {
                    var relationship = (OrganisationContactRelationship) partyRelationship;
                    return relationship.Contact;
                }
                if (Equals(this.PartyRelationshipWhereCommunicationEvent.GetType().Name, PartnershipClass.Instance.Name))
                {
                    var relationship = (Partnership) partyRelationship;
                    return relationship.Partner;
                }
                if (Equals(this.PartyRelationshipWhereCommunicationEvent.GetType().Name,
                    ProfessionalServicesRelationshipClass.Instance.Name))
                {
                    var relationship = (ProfessionalServicesRelationship) partyRelationship;
                    return relationship.Professional;
                }
                if (Equals(this.PartyRelationshipWhereCommunicationEvent.GetType().Name,
                    ProspectRelationshipClass.Instance.Name))
                {
                    var relationship = (ProspectRelationship) partyRelationship;
                    return relationship.Prospect;
                }
                if (Equals(this.PartyRelationshipWhereCommunicationEvent.GetType().Name,
                    SalesRepCommissionClass.Instance.Name))
                {
                    var relationship = (SalesRepCommission) partyRelationship;
                    return relationship.SalesRep;
                }
                if (Equals(this.PartyRelationshipWhereCommunicationEvent.GetType().Name,
                    SubContractorRelationshipClass.Instance.Name))
                {
                    var relationship = (SubContractorRelationship) partyRelationship;
                    return relationship.SubContractor;
                }
                if (Equals(this.PartyRelationshipWhereCommunicationEvent.GetType().Name,
                    SupplierRelationshipClass.Instance.Name))
                {
                    var relationship = (SupplierRelationship) partyRelationship;
                    return relationship.Supplier;
                }
            }

            return null;
        }

        public void AppsOnDeriveInvolvedParties()
        {
            this.RemoveInvolvedParties();
            this.AddInvolvedParty(this.Owner);

            foreach (Party party in this.FromParties)
            {
                this.AddInvolvedParty(party);
            }

            foreach (Party party in this.ToParties)
            {
                this.AddInvolvedParty(party);
            }
        }
    }
}
