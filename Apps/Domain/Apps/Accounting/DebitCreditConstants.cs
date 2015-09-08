// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebitCreditConstants.cs" company="Allors bvba">
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

    public partial class DebitCreditConstants
    {
        public static readonly Guid DebitId = new Guid("C957ED48-7A31-4308-8CC5-03C8014A8646");
        public static readonly Guid CreditId = new Guid("BECDF0E7-C2DD-4ddf-A1A0-FC5E9E15F0A8");

        private UniquelyIdentifiableCache<DebitCreditConstant> cache;

        public DebitCreditConstant Debit
        {
            get { return this.Cache.Get(DebitId); }
        }

        public DebitCreditConstant Credit
        {
            get { return this.Cache.Get(CreditId); }
        }

        private UniquelyIdentifiableCache<DebitCreditConstant> Cache
        {
            get
            {
                return this.cache ?? (this.cache = new UniquelyIdentifiableCache<DebitCreditConstant>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new DebitCreditConstantBuilder(this.Session)
                .WithName("Debit")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Debit").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Debet").WithLocale(dutchLocale).Build())
                .WithUniqueId(DebitId).Build();
            
            new DebitCreditConstantBuilder(this.Session)
                .WithName("Credit")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Credit").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Credit").WithLocale(dutchLocale).Build())
                .WithUniqueId(CreditId)
                .Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}