// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoice.v.cs" company="Allors bvba">
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
    public partial class SalesInvoice
    {
        public string SalesRepNames()
        {
            return this.AppsSalesRepNames;
        }

        public void DeriveLocale(IDerivation derivation)
        {
            this.AppsOnDeriveLocale(derivation);
        }

        public void DeriveInvoiceTotals(IDerivation derivation)
        {
            this.AppsOnDeriveInvoiceTotals(derivation);
        }

        public void DeriveTemplate(IDerivation derivation)
        {
            this.AppsOnDeriveTemplate(derivation);
        }

        public void DeriveSalesOrderPaymentStatus(IDerivation derivation)
        {
            this.AppsOnDeriveSalesOrderPaymentStatus(derivation);
        }

        public void DeriveCustomers(IDerivation derivation)
        {
            this.AppsOnDeriveCustomers(derivation);
        }

        public void DeriveMarkupAndProfitMargin(IDerivation derivation)
        {
            this.AppsOnDeriveMarkupAndProfitMargin(derivation);
        }

        public void DeriveSalesReps(IDerivation derivation)
        {
            this.AppsOnDeriveSalesReps(derivation);
        }

        public void DeriveInvoiceItems(IDerivation derivation)
        {
            this.AppsOnDeriveInvoiceItems(derivation);
        }
    }
}