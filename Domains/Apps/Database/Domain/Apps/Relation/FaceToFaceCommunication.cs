// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaceToFaceCommunication.cs" company="Allors bvba">
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
    
    public partial class FaceToFaceCommunication
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.FaceToFaceCommunication, M.FaceToFaceCommunication.CommunicationEventState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.Participants.Count <= 1)
            {
                this.Delete();
            }

            this.FromParties = this.Participants;
            this.ToParties = this.Participants;
            this.AppsOnDeriveInvolvedParties(derivation);
        }

        public void AppsOnDeriveInvolvedParties(IDerivation derivation)
        {
            this.InvolvedParties = this.Participants;

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
