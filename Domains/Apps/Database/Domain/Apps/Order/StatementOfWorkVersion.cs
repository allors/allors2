// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatementOfWorkVersion.cs" company="Allors bvba">
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
    public partial class StatementOfWorkVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (StatementOfWorkVersionBuilder) method.Builder;
            var statementOfWork = builder.StatementOfWork;

            if (statementOfWork != null)
            {
                this.InternalComment = statementOfWork.InternalComment;
                this.RequiredResponseDate = statementOfWork.RequiredResponseDate;
                this.ValidFromDate = statementOfWork.ValidFromDate;
                this.QuoteTerms = statementOfWork.QuoteTerms;
                this.Issuer = statementOfWork.Issuer;
                this.ValidThroughDate = statementOfWork.ValidThroughDate;
                this.Description = statementOfWork.Description;
                this.Receiver = statementOfWork.Receiver;
                this.FullfillContactMechanism = statementOfWork.FullfillContactMechanism;
                this.Amount = statementOfWork.Amount;
                this.Currency = statementOfWork.Currency;
                this.IssueDate = statementOfWork.IssueDate;
                this.QuoteItems = statementOfWork.QuoteItems;
                this.QuoteNumber = statementOfWork.QuoteNumber;
                this.Request = statementOfWork.Request;
                this.CurrentObjectState = statementOfWork.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}