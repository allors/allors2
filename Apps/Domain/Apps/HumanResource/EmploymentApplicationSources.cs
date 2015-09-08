// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmploymentApplicationSources.cs" company="Allors bvba">
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

    public partial class EmploymentApplicationSources
    {
        public static readonly Guid NewsPaperId = new Guid("206E641B-DAC1-4b2e-9DD4-E4770AF09B9F");
        public static readonly Guid PersonallReferalId = new Guid("C7029F05-6CCD-4639-A497-A9D8320549D7");
        public static readonly Guid InternetId = new Guid("7931D959-4396-492d-90E4-C44632F2E3EA");

        private UniquelyIdentifiableCache<EmploymentApplicationSource> cache;

        public EmploymentApplicationSource NewsPaper
        {
            get { return this.Cache.Get(NewsPaperId); }
        }

        public EmploymentApplicationSource PersonallReferal
        {
            get { return this.Cache.Get(PersonallReferalId); }
        }

        public EmploymentApplicationSource Internet
        {
            get { return this.Cache.Get(InternetId); }
        }

        private UniquelyIdentifiableCache<EmploymentApplicationSource> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<EmploymentApplicationSource>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new EmploymentApplicationSourceBuilder(this.Session)
                .WithName("NewsPaper")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("NewsPaper").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Krant").WithLocale(dutchLocale).Build())
                .WithUniqueId(NewsPaperId)
                .Build();
            
            new EmploymentApplicationSourceBuilder(this.Session)
                .WithName("PersonallReferal")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("PersonallReferal").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Persoonlijk doorverwezen").WithLocale(dutchLocale).Build())
                .WithUniqueId(PersonallReferalId).Build();
            
            new EmploymentApplicationSourceBuilder(this.Session)
                .WithName("Internet")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Internet").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Internet").WithLocale(dutchLocale).Build())
                .WithUniqueId(InternetId)
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
