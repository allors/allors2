// <copyright file="ReceiverModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
