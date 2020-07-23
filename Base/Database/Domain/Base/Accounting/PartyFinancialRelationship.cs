// <copyright file="PartyFinancialRelationship.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

    public partial class PartyFinancialRelationship
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            iteration.AddDependency(this, this.Party.SalesOrdersWhereBillToCustomer);
            iteration.AddDependency(this, this.Party.SalesInvoicesWhereBillToCustomer);
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var party = this.Party;

            this.AmountDue = 0;
            this.AmountOverDue = 0;

            // Open Order Amount
            this.OpenOrderAmount = party.SalesOrdersWhereBillToCustomer
                .Where(v =>
                    Equals(v.TakenBy, this.InternalOrganisation) &&
                    !v.SalesOrderState.Equals(new SalesOrderStates(party.Strategy.Session).Finished) &&
                    !v.SalesOrderState.Equals(new SalesOrderStates(party.Strategy.Session).Cancelled))
                .Sum(v => v.TotalIncVat);

            // Amount Due
            // Amount OverDue
            foreach (var salesInvoice in party.SalesInvoicesWhereBillToCustomer.Where(v => Equals(v.BilledFrom, this.InternalOrganisation) &&
                                                                                                    !v.SalesInvoiceState.Equals(new SalesInvoiceStates(party.Strategy.Session).Paid)))
            {
                if (salesInvoice.AmountPaid > 0)
                {
                    this.AmountDue += salesInvoice.TotalIncVat - salesInvoice.AmountPaid;
                }
                else
                {
                    foreach (SalesInvoiceItem invoiceItem in salesInvoice.InvoiceItems)
                    {
                        if (!invoiceItem.SalesInvoiceItemState.Equals(
                            new SalesInvoiceItemStates(party.Strategy.Session).Paid))
                        {
                            if (invoiceItem.ExistTotalIncVat)
                            {
                                this.AmountDue += invoiceItem.TotalIncVat - invoiceItem.AmountPaid;
                            }
                        }
                    }
                }

                var gracePeriod = salesInvoice.Store?.PaymentGracePeriod;

                if (salesInvoice.DueDate.HasValue)
                {
                    var dueDate = salesInvoice.DueDate.Value;

                    if (gracePeriod.HasValue)
                    {
                        dueDate = salesInvoice.DueDate.Value.AddDays(gracePeriod.Value);
                    }

                    if (party.Strategy.Session.Now() > dueDate)
                    {
                        this.AmountOverDue += salesInvoice.TotalIncVat - salesInvoice.AmountPaid;
                    }
                }
            }



            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistInternalOrganisation && internalOrganisations.Count() == 1)
            {
                this.InternalOrganisation = internalOrganisations.First();
            }

            this.Parties = new Party[] { this.Party, this.InternalOrganisation };
        }
    }
}
