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
        private static readonly Guid PurchaseReturnId = new Guid("0187D927-81F5-4d6a-9847-58B674AD3E6A");

        private UniquelyIdentifiableSticky<PurchaseInvoiceType> cache;

        public PurchaseInvoiceType PurchaseInvoice => this.Cache[PurchaseInvoiceId];

        public PurchaseInvoiceType PurchaseReturn => this.Cache[PurchaseReturnId];

        private UniquelyIdentifiableSticky<PurchaseInvoiceType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<PurchaseInvoiceType>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new PurchaseInvoiceTypeBuilder(this.Session)
                .WithName("Purchase invoice")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Aankoop factuur").WithLocale(dutchLocale).Build())
                .WithUniqueId(PurchaseInvoiceId)
                .WithIsActive(true)
                .Build();
            
            new PurchaseInvoiceTypeBuilder(this.Session)
                .WithName("Purchase return")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Aankoop factuur retour").WithLocale(dutchLocale).Build())
                .WithUniqueId(PurchaseReturnId)
                .WithIsActive(true)
                .Build();
        }
    }
}
