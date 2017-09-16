// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestForQuoteVersion.cs" company="Allors bvba">
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
    public partial class RequestForQuoteVersion
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            var builder = (RequestForQuoteVersionBuilder) method.Builder;
            var requestForQuote = builder.RequestForQuote;

            if (requestForQuote != null)
            {
                this.InternalComment = requestForQuote.InternalComment;
                this.Description = requestForQuote.Description;
                this.RequestDate = requestForQuote.RequestDate;
                this.RequiredResponseDate = requestForQuote.RequiredResponseDate;
                this.RequestItems = requestForQuote.RequestItems;
                this.RequestNumber = requestForQuote.RequestNumber;
                this.RespondingParties = requestForQuote.RespondingParties;
                this.Originator = requestForQuote.Originator;
                this.Currency = requestForQuote.Currency;
                this.FullfillContactMechanism = requestForQuote.FullfillContactMechanism;
                this.EmailAddress = requestForQuote.EmailAddress;
                this.TelephoneNumber = requestForQuote.TelephoneNumber;
                this.TelephoneCountryCode = requestForQuote.TelephoneCountryCode;
                this.CurrentObjectState = requestForQuote.CurrentObjectState;
            }

            if (!this.ExistTimeStamp)
            {
                this.TimeStamp = this.strategy.Session.Now();
            }
        }
    }
}