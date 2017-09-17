// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductQuote.cs" company="Allors bvba">
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
    public partial class ProductQuote
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            var isNewVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.InternalComment, this.CurrentVersion.InternalComment);

            var isNewStateVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.InternalComment, this.CurrentVersion.InternalComment) ||
                !object.Equals(this.RequiredResponseDate, this.CurrentVersion.RequiredResponseDate) ||
                !object.Equals(this.ValidFromDate, this.CurrentVersion.ValidFromDate) ||
                !object.Equals(this.QuoteTerms, this.CurrentVersion.QuoteTerms) ||
                !object.Equals(this.Issuer, this.CurrentVersion.Issuer) ||
                !object.Equals(this.ValidThroughDate, this.CurrentVersion.ValidThroughDate) ||
                !object.Equals(this.Description, this.CurrentVersion.Description) ||
                !object.Equals(this.Receiver, this.CurrentVersion.Receiver) ||
                !object.Equals(this.FullfillContactMechanism, this.CurrentVersion.FullfillContactMechanism) ||
                !object.Equals(this.Amount, this.CurrentVersion.Amount) ||
                !object.Equals(this.Currency, this.CurrentVersion.Currency) ||
                !object.Equals(this.IssueDate, this.CurrentVersion.IssueDate) ||
                !object.Equals(this.QuoteItems, this.CurrentVersion.QuoteItems) ||
                !object.Equals(this.QuoteNumber, this.CurrentVersion.QuoteNumber) ||
                !object.Equals(this.Request, this.CurrentVersion.Request) ||
                !object.Equals(this.CurrentObjectState, this.CurrentVersion.CurrentObjectState);

            if (isNewVersion)
            {
                this.PreviousVersion = this.CurrentVersion;
                this.CurrentVersion = new ProductQuoteVersionBuilder(this.Strategy.Session).WithProductQuote(this).Build();
                this.AddAllVersion(this.CurrentVersion);
            }

            if (isNewStateVersion)
            {
                this.CurrentStateVersion = CurrentVersion;
                this.AddAllStateVersion(this.CurrentStateVersion);
            }
        }
    }
}
