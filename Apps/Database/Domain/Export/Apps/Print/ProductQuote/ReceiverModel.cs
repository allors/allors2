// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReceiverModel.cs" company="Allors bvba">
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

namespace Allors.Domain.Print.ProductQuoteModel
{
    public class ReceiverModel
    {
        public ReceiverModel(ProductQuote quote)
        {
            var receiver = quote.Receiver;
            var organisationReceiver = quote.Receiver as Organisation;

            if (receiver != null)
            {
                this.Name = receiver.PartyName;
                this.TaxId = organisationReceiver?.TaxNumber;
            }

            this.Contact = quote.ContactPerson?.PartyName;
        }

        public string Name { get; }

        public string Contact { get; }

        public string TaxId { get; }
    }
}
