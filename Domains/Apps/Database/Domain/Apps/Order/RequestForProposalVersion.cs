// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestForProposalVersion.cs" company="Allors bvba">
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
    public partial class RequestForProposalVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (RequestForProposalVersionBuilder) method.Builder;
            var requestForProposal = builder.RequestForProposal;

            if (requestForProposal != null)
            {
                this.InternalComment = requestForProposal.InternalComment;
                this.Description = requestForProposal.Description;
                this.RequestDate = requestForProposal.RequestDate;
                this.RequiredResponseDate = requestForProposal.RequiredResponseDate;
                this.RequestItems = requestForProposal.RequestItems;
                this.RequestNumber = requestForProposal.RequestNumber;
                this.RespondingParties = requestForProposal.RespondingParties;
                this.Originator = requestForProposal.Originator;
                this.Currency = requestForProposal.Currency;
                this.FullfillContactMechanism = requestForProposal.FullfillContactMechanism;
                this.EmailAddress = requestForProposal.EmailAddress;
                this.TelephoneNumber = requestForProposal.TelephoneNumber;
                this.TelephoneCountryCode = requestForProposal.TelephoneCountryCode;
                this.CurrentObjectState = requestForProposal.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}