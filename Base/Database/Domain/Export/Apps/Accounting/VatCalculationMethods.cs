// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VatCalculationMethods.cs" company="Allors bvba">
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

    public partial class VatCalculationMethods
    {
        private static readonly Guid CashId = new Guid("0EBDC40A-B744-4518-8DEB-F060DEDB0FE6");
        private static readonly Guid InvoiceId = new Guid("180E1FEA-929E-46F4-9F5E-16E1DF60065F");

        private UniquelyIdentifiableSticky<VatCalculationMethod> cache;

        public VatCalculationMethod Cash => this.Cache[CashId];

        public VatCalculationMethod Invoice => this.Cache[InvoiceId];

        private UniquelyIdentifiableSticky<VatCalculationMethod> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<VatCalculationMethod>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new VatCalculationMethodBuilder(this.Session)
                .WithName("Cash management scheme")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kasstelsel").WithLocale(dutchLocale).Build())
                .WithUniqueId(CashId)
                .WithIsActive(true)
                .Build();

            new VatCalculationMethodBuilder(this.Session)
                .WithName("Incoice management scheme")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Factuurstelsel").WithLocale(dutchLocale).Build())
                .WithUniqueId(InvoiceId)
                .WithIsActive(true)
                .Build();
        }
    }
}