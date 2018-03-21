// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoiceTypes.cs" company="Allors bvba">
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

    public partial class PurchaseInvoiceTypes
    {
        private static readonly Guid PurchaseInvoiceId = new Guid("D08F0309-A4CB-4ab7-8F75-3BB11DCF3783");
        private static readonly Guid CustomerReturnId = new Guid("0187D927-81F5-4d6a-9847-58B674AD3E6A");
        private static readonly Guid CommissionId = new Guid("92DF3246-CC44-4ab8-94F1-E0039261EA60");
        private static readonly Guid PayrollId = new Guid("5C637EA9-5F0A-4bd7-80BF-FFBA01EC1756");

        private UniquelyIdentifiableSticky<PurchaseInvoiceType> cache;

        public PurchaseInvoiceType PurchaseInvoice => this.Cache[PurchaseInvoiceId];

        public PurchaseInvoiceType CustomerReturn => this.Cache[CustomerReturnId];

        public PurchaseInvoiceType Commission => this.Cache[CommissionId];

        public PurchaseInvoiceType Payroll => this.Cache[PayrollId];

        private UniquelyIdentifiableSticky<PurchaseInvoiceType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<PurchaseInvoiceType>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new PurchaseInvoiceTypeBuilder(this.Session)
                .WithName("Purchase invoice")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Aankoop factuur").WithLocale(dutchLocale).Build())
                .WithUniqueId(PurchaseInvoiceId)
                .Build();
            
            new PurchaseInvoiceTypeBuilder(this.Session)
                .WithName("Customer return")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Crediet nota retour").WithLocale(dutchLocale).Build())
                .WithUniqueId(CustomerReturnId)
                .Build();
            
            new PurchaseInvoiceTypeBuilder(this.Session)
                .WithName("Commission")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Commissie").WithLocale(dutchLocale).Build())
                .WithUniqueId(CommissionId)
                .Build();
            
            new PurchaseInvoiceTypeBuilder(this.Session)
                .WithName("Payroll")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Salaris").WithLocale(dutchLocale).Build())
                .WithUniqueId(PayrollId)
                .Build();
        }
    }
}
