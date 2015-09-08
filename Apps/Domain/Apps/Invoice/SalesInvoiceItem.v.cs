// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoiceItem.v.cs" company="Allors bvba">
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
    public partial class SalesInvoiceItem
    {
        public void Cancel(IDerivation derivation)
        {
            this.AppsCancel(derivation);
        }

        public void WriteOff(IDerivation derivation)
        {
            this.AppsWriteOff(derivation);
        }

        public void PaymentReceived(IDerivation derivation)
        {
            this.AppsPaymentReceived(derivation);
        }

        public void DeriveCurrentPaymentStatus(IDerivation derivation)
        {
            this.AppsOnDeriveCurrentPaymentStatus(derivation);
        }

        public void DeriveAmountPaid(IDerivation derivation)
        {
            this.AppsOnDeriveAmountPaid(derivation);
        }

        public void DeriveCurrentObjectState(IDerivation derivation)
        {
            this.AppsOnDeriveCurrentObjectState(derivation);
        }

        public void DerivePrices(IDerivation derivation, decimal quantityInvoiced = 0, decimal totalBasePrice = 0)
        {
            this.AppsOnDerivePrices(derivation, quantityInvoiced, totalBasePrice);
        }

        public void DeriveVatRate(IDerivation derivation)
        {
            this.AppsOnDeriveVatRate(derivation);
        }

        public void DeriveVatRegime(IDerivation derivation)
        {
            this.AppsOnDeriveVatRegime(derivation);
        }

        public void DeriveMarkupAndProfitMargin(IDerivation derivation)
        {
            this.AppsOnDeriveMarkupAndProfitMargin(derivation);
        }

        public void DeriveSalesRep(IDerivation derivation)
        {
            this.AppsOnDeriveSalesRep(derivation);
        }
    }
}