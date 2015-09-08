// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CostCenterSplitMethods.cs" company="Allors bvba">
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

    public partial class CostCenterSplitMethods
    {
        public static readonly Guid Topid = new Guid("844444D0-5D69-4A3B-9CB6-0D98747839D8");
        public static readonly Guid BottomId = new Guid("13AE2935-F926-426F-BB1C-979BE7F3DF0B");

        private UniquelyIdentifiableCache<CostCenterSplitMethod> cache;

        public CostCenterSplitMethod Top
        {
            get { return this.Cache.Get(Topid); }
        }

        public CostCenterSplitMethod Bottom
        {
            get { return this.Cache.Get(BottomId); }
        }

        private UniquelyIdentifiableCache<CostCenterSplitMethod> Cache
        {
            get
            {
                return this.cache ?? (this.cache = new UniquelyIdentifiableCache<CostCenterSplitMethod>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new DebitCreditConstantBuilder(this.Session)
                .WithName("Use top level큦 cost center GL-account")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Use top level큦 cost center GL-account").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gebruik grootboekrekening van kostenplaats van hoogste niveau").WithLocale(dutchLocale).Build())
                .WithUniqueId(Topid).Build();
            
            new DebitCreditConstantBuilder(this.Session)
                .WithName("Use bottom level큦 cost center GL-account")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Use bottom level큦 cost center GL-account").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gebruik grootboekrekening van kostenplaats van laagste niveau").WithLocale(dutchLocale).Build())
                .WithUniqueId(BottomId)
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