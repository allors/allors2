// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmploymentApplicationSources.cs" company="Allors bvba">
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

    public partial class EmploymentApplicationSources
    {
        private static readonly Guid NewsPaperId = new Guid("206E641B-DAC1-4b2e-9DD4-E4770AF09B9F");
        private static readonly Guid PersonallReferalId = new Guid("C7029F05-6CCD-4639-A497-A9D8320549D7");
        private static readonly Guid InternetId = new Guid("7931D959-4396-492d-90E4-C44632F2E3EA");

        private UniquelyIdentifiableSticky<EmploymentApplicationSource> cache;

        public EmploymentApplicationSource NewsPaper => this.Cache[NewsPaperId];

        public EmploymentApplicationSource PersonallReferal => this.Cache[PersonallReferalId];

        public EmploymentApplicationSource Internet => this.Cache[InternetId];

        private UniquelyIdentifiableSticky<EmploymentApplicationSource> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<EmploymentApplicationSource>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new EmploymentApplicationSourceBuilder(this.Session)
                .WithName("NewsPaper")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Krant").WithLocale(dutchLocale).Build())
                .WithUniqueId(NewsPaperId)
                .WithIsActive(true)
                .Build();
            
            new EmploymentApplicationSourceBuilder(this.Session)
                .WithName("PersonallReferal")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Persoonlijk doorverwezen").WithLocale(dutchLocale).Build())
                .WithUniqueId(PersonallReferalId)
                .WithIsActive(true)
                .Build();
            
            new EmploymentApplicationSourceBuilder(this.Session)
                .WithName("Internet")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Internet").WithLocale(dutchLocale).Build())
                .WithUniqueId(InternetId)
                .WithIsActive(true)
                .Build();
        }
    }
}
