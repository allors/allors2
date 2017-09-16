// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProposalVersion.cs" company="Allors bvba">
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
    public partial class ProposalVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (ProposalVersionBuilder) method.Builder;
            var proposal = builder.Proposal;

            if (proposal != null)
            {
                this.InternalComment = proposal.InternalComment;
                this.RequiredResponseDate = proposal.RequiredResponseDate;
                this.ValidFromDate = proposal.ValidFromDate;
                this.QuoteTerms = proposal.QuoteTerms;
                this.Issuer = proposal.Issuer;
                this.ValidThroughDate = proposal.ValidThroughDate;
                this.Description = proposal.Description;
                this.Receiver = proposal.Receiver;
                this.FullfillContactMechanism = proposal.FullfillContactMechanism;
                this.Amount = proposal.Amount;
                this.Currency = proposal.Currency;
                this.IssueDate = proposal.IssueDate;
                this.QuoteItems = proposal.QuoteItems;
                this.QuoteNumber = proposal.QuoteNumber;
                this.Request = proposal.Request;
                this.CurrentObjectState = proposal.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}