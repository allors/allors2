// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaritalStatuses.cs" company="Allors bvba">
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

    public partial class MaritalStatuses
    {
        public static readonly Guid SingleId = new Guid("83CD5BBD-85A4-417d-A3E0-5DAA509ACE23");
        public static readonly Guid MarriedId = new Guid("487A649F-DF55-43c1-8B66-3964A056DF2A");
        public static readonly Guid DivorcedId = new Guid("B6DE4339-76B0-4675-802D-20AAA482E30F");
        public static readonly Guid WidowedId = new Guid("C3755D4A-FF2E-4c4c-B17E-BCC5BC0599BF");

        private UniquelyIdentifiableCache<MaritalStatus> cache;

        public MaritalStatus Single
        {
            get { return this.Cache.Get(SingleId); }
        }

        public MaritalStatus Married
        {
            get { return this.Cache.Get(MarriedId); }
        }

        public MaritalStatus Divorced
        {
            get { return this.Cache.Get(DivorcedId); }
        }

        public MaritalStatus Widowed
        {
            get { return this.Cache.Get(WidowedId); }
        }

        private UniquelyIdentifiableCache<MaritalStatus> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<MaritalStatus>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new MaritalStatusBuilder(this.Session)
                .WithName("Single")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Single").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Alleenstaand").WithLocale(dutchLocale).Build())
                .WithUniqueId(SingleId)
                .Build();
            
            new MaritalStatusBuilder(this.Session)
                .WithName("Married")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Married").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gehuwd").WithLocale(dutchLocale).Build())
                .WithUniqueId(MarriedId)
                .Build();
            
            new MaritalStatusBuilder(this.Session)
                .WithName("Divorced")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Divorced").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gescheiden").WithLocale(dutchLocale).Build())
                .WithUniqueId(DivorcedId)
                .Build();
            
            new MaritalStatusBuilder(this.Session)
                .WithName("Widowed")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Widowed").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Weduw(e)(naar)").WithLocale(dutchLocale).Build())
                .WithUniqueId(WidowedId)
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
