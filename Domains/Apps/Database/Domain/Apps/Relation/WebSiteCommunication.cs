// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebSiteCommunication.cs" company="Allors bvba">
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
namespace Allors.Domain
{
    public partial class WebSiteCommunication
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.AppsOnDeriveFromParties();
            this.AppsOnDeriveToParties();
            this.AppsOnDeriveInvolvedParties(derivation);
        }

        public void AppsOnDeriveFromParties()
        {
            this.RemoveFromParties();
            this.AddFromParty(this.Originator);
        }

        public void AppsOnDeriveToParties()
        {
            this.RemoveToParties();
            this.AddToParty(this.Receiver);
        }

        public void AppsOnDeriveInvolvedParties(IDerivation derivation)
        {
            this.RemoveInvolvedParties();
            if (this.ExistOwner)
            {
                this.AddInvolvedParty(this.Owner);
            }

            this.AddInvolvedParty(this.Originator);
            this.AddInvolvedParty(this.Receiver);

            if (this.ExistPartyRelationshipWhereCommunicationEvent)
            {
                foreach (Party party in this.PartyRelationshipWhereCommunicationEvent.Parties)
                {
                    this.AddInvolvedParty(party);
                }
            }
        }
    }
}
