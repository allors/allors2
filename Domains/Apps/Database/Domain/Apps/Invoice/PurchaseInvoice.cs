// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoice.cs" company="Allors bvba">
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
    using System;
    using Meta;
    using Resources;

    public partial class PurchaseInvoice
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public InvoiceItem[] InvoiceItems => this.PurchaseInvoiceItems;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCurrentObjectState)
            {
                this.CurrentObjectState = new PurchaseInvoiceObjectStates(this.Strategy.Session).InProcess;
            }

            if (!this.ExistInvoiceNumber)
            {
                this.InvoiceNumber = Singleton.Instance(this).InternalOrganisation.DeriveNextPurchaseInvoiceNumber();
            }

            if (!this.ExistInvoiceDate)
            {
                this.InvoiceDate = DateTime.UtcNow;
            }

            if (!this.ExistEntryDate)
            {
                this.EntryDate = DateTime.UtcNow;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
            var internalOrganisation = Singleton.Instance(this);

            // TODO:
            if (derivation.HasChangedRoles(this))
            {
                derivation.AddDependency(this, internalOrganisation);
            }

            if (this.ExistBilledFromParty)
            {
                var supplier = this.BilledFromParty as Organisation;
                if (supplier != null)
                {
                    // TODO: Isn't this too broad?
                    foreach (Organisation supplierRelationship in new Organisations(this.strategy.Session).Suppliers)
                    {
                        derivation.AddDependency(this, supplierRelationship);
                    }
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            this.AppsOnDeriveInvoiceItems(derivation);
            this.AppsOnDeriveInvoiceTotals();
        }

        public void AppsOnDeriveInvoiceTotals()
        {
            if (this.ExistPurchaseInvoiceItems)
            {
                this.TotalBasePrice = 0;
                this.TotalDiscount = 0;
                this.TotalSurcharge = 0;
                this.TotalVat = 0;
                this.TotalExVat = 0;
                this.TotalIncVat = 0;

                foreach (PurchaseInvoiceItem item in this.PurchaseInvoiceItems)
                {
                    this.TotalBasePrice += item.TotalBasePrice;
                    this.TotalSurcharge += item.TotalSurcharge;
                    this.TotalSurcharge += item.TotalSurcharge;
                    this.TotalVat += item.TotalVat;
                    this.TotalExVat += item.TotalExVat;
                    this.TotalIncVat += item.TotalIncVat;
                }
            }
        }

        public void AppsSearchDataApprove(IDerivation derivation)
        {
            this.CurrentObjectState = new PurchaseInvoiceObjectStates(this.Strategy.Session).Approved;
        }

        public void AppsReady(IDerivation derivation)
        {
            this.CurrentObjectState = new PurchaseInvoiceObjectStates(this.Strategy.Session).ReadyForPosting;
        }

        public void AppsCancel(IDerivation derivation)
        {
            this.CurrentObjectState = new PurchaseInvoiceObjectStates(this.Strategy.Session).Cancelled;
        }

        public void AppsOnDeriveInvoiceItems(IDerivation derivation)
        {
            foreach (PurchaseInvoiceItem purchaseInvoiceItem in this.PurchaseInvoiceItems)
            {
                purchaseInvoiceItem.AppsOnDerivePrices();
            }
        }
    }
}
