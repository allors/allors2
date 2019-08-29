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

using System.Collections.Generic;
using System.Linq;

namespace Allors.Domain.Print.ProductQuoteModel
{
    public class ReceiverModel
    {
        public ReceiverModel(ProductQuote quote, Dictionary<string, byte[]> imageByImageName)
        {
            var session = quote.Strategy.Session;

            var receiver = quote.Receiver;
            var organisationReceiver = quote.Receiver as Organisation;

            if (receiver != null)
            {
                this.Name = receiver.PartyName;
                this.TaxId = organisationReceiver?.TaxNumber;
            }

            this.Contact = quote.ContactPerson?.PartyName;
            this.ContactFirstName = quote.ContactPerson?.FirstName;
            this.Salutation = quote.ContactPerson?.Salutation?.Name;
            this.ContactFunction = quote.ContactPerson?.Function;

            if (quote.ContactPerson?.CurrentPartyContactMechanisms.FirstOrDefault(v => v.ContactMechanism.GetType().Name == typeof(EmailAddress).Name)?.ContactMechanism is EmailAddress emailAddress)
            {
                this.ContactEmail = emailAddress.ElectronicAddressString;
            }

            if (quote.ContactPerson?.CurrentPartyContactMechanisms.FirstOrDefault(v => v.ContactMechanism.GetType().Name == typeof(TelecommunicationsNumber).Name)?.ContactMechanism is TelecommunicationsNumber phone)
            {
                this.ContactPhone = phone.ToString();
            }
        }

        public string Name { get; }

        public string Salutation { get; }

        public string Contact { get; }

        public string ContactFirstName { get; }

        public string ContactFunction { get; }

        public string ContactEmail { get; }

        public string ContactPhone { get; }

        public string TaxId { get; }
    }
}
