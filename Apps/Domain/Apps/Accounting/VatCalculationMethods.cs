// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VatCalculationMethods.cs" company="Allors bvba">
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

    public partial class VatCalculationMethods
    {
        public static readonly Guid CashId = new Guid("0EBDC40A-B744-4518-8DEB-F060DEDB0FE6");
        public static readonly Guid InvoiceId = new Guid("180E1FEA-929E-46F4-9F5E-16E1DF60065F");

        private UniquelyIdentifiableCache<VatCalculationMethod> cache;

        public VatCalculationMethod Cash
        {
            get { return this.Cache.Get(CashId); }
        }

        public VatCalculationMethod Invoice
        {
            get { return this.Cache.Get(InvoiceId); }
        }

        private UniquelyIdentifiableCache<VatCalculationMethod> Cache
        {
            get
            {
                return this.cache ?? (this.cache = new UniquelyIdentifiableCache<VatCalculationMethod>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new VatCalculationMethodBuilder(this.Session)
                .WithName("Cash management scheme")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Cash management scheme").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kasstelsel").WithLocale(dutchLocale).Build())
                .WithUniqueId(CashId).Build();

            new VatCalculationMethodBuilder(this.Session)
                .WithName("Incoice management scheme")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Incoice management scheme").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Factuurstelsel").WithLocale(dutchLocale).Build())
                .WithUniqueId(InvoiceId).Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}