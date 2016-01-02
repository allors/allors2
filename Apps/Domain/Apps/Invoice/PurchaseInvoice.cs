// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoice.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Globalization;
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

            if (!this.ExistInvoiceNumber && this.ExistBilledToInternalOrganisation)
            {
                this.InvoiceNumber = this.BilledToInternalOrganisation.DeriveNextPurchaseInvoiceNumber();
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

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                if (this.ExistBilledToInternalOrganisation)
                {
                    derivation.AddDependency(this, this.BilledToInternalOrganisation);
                }
            }

            if (this.ExistBilledFromParty)
            {
                var supplier = this.BilledFromParty as Organisation;
                if (supplier != null)
                {
                    var supplierRelationships = supplier.SupplierRelationshipsWhereSupplier;
                    supplierRelationships.Filter.AddEquals(SupplierRelationships.Meta.InternalOrganisation, this.BilledToInternalOrganisation);

                    foreach (SupplierRelationship supplierRelationship in supplierRelationships)
                    {
                        if (supplierRelationship.FromDate <= DateTime.UtcNow && (!supplierRelationship.ExistThroughDate || supplierRelationship.ThroughDate >= DateTime.UtcNow))
                        {
                            derivation.AddDependency(this, supplierRelationship);
                        }
                    }
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            Organisation supplier = this.BilledFromParty as Organisation;
            if (supplier != null && this.ExistBilledToInternalOrganisation)
            {
                if (!this.BilledToInternalOrganisation.Equals(supplier.InternalOrganisationWhereSupplier))
                {
                    derivation.Log.AddError(this, PurchaseInvoices.Meta.BilledFromParty, ErrorMessages.PartyIsNotASupplier);
                }
            }

            if (this.ExistCurrentObjectState && !this.CurrentObjectState.Equals(this.LastObjectState))
            {
                this.CurrentObjectState.Process(this);

                var currentStatus = new PurchaseInvoiceStatusBuilder(this.Strategy.Session).WithPurchaseInvoiceObjectState(this.CurrentObjectState).Build();
                this.AddInvoiceStatus(currentStatus);
                this.CurrentInvoiceStatus = currentStatus;
            }

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
