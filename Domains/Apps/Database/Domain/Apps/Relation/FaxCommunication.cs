// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaxCommunication.cs" company="Allors bvba">
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
    using Allors.Meta;

    public partial class FaxCommunication
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.FaxCommunication.CommunicationEventState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void AppsOnDerive(ObjectOnDerive method)
        {
            this.AppsOnDeriveFromParties();
            this.AppsOnDeriveToParties();
            this.AppsOnDeriveInvolvedParties();

            if (!this.ExistOriginator || !this.ExistReceiver)
            {
                this.Delete();
            }
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

        public void AppsOnDeriveInvolvedParties()
        {
            this.RemoveInvolvedParties();

            if (this.ExistOriginator)
            {
                this.AddInvolvedParty(this.Originator);
            }

            if (this.ExistReceiver)
            {
                this.AddInvolvedParty(this.Receiver);
            }

            if (this.ExistOwner && !this.InvolvedParties.Contains(this.Owner))
            {
                this.AddInvolvedParty(this.Owner);
            }

            foreach (Party party in this.PartiesWhereCommunicationEvent)
            {
                this.AddInvolvedParty(party);
            }
        }
    }
}
