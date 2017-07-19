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
namespace Allors.Domain
{
    public partial class LetterCorrespondence
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

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

        public void AppsOnDeriveInvolvedParties()
        {
            this.RemoveInvolvedParties();
            this.InvolvedParties = this.Receivers;

            foreach (Party originator in this.Originators)
            {
                this.AddInvolvedParty(originator);
            }

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
