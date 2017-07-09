// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonRoles.cs" company="Allors bvba">
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

    public partial class PersonRoles
    {
        private static readonly Guid EmployeeId = new Guid("DB06A3E1-6146-4C18-A60D-DD10E19F7243");
        private static readonly Guid ContactId = new Guid("FA2DF11E-7795-4DF7-8B3F-4FD87D0C4D8E");
        private static readonly Guid CustomerId = new Guid("B29444EF-0950-4D6F-AB3E-9C6DC44C050F");

        private UniquelyIdentifiableCache<PersonRole> cache;

        public PersonRole Employee => this.Cache.Get(EmployeeId);

        public PersonRole Contact => this.Cache.Get(ContactId);

        public PersonRole Customer => this.Cache.Get(CustomerId);

        private UniquelyIdentifiableCache<PersonRole> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableCache<PersonRole>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new PersonRoleBuilder(this.Session)
                .WithName("Employee")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Employee").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Werknemer").WithLocale(dutchLocale).Build())
                .WithUniqueId(EmployeeId)
                .Build();

            new PersonRoleBuilder(this.Session)
                .WithName("Contact")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Contact").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Contact").WithLocale(dutchLocale).Build())
                .WithUniqueId(ContactId)
                .Build();

            new PersonRoleBuilder(this.Session)
                .WithName("Customer")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Customer").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Klant").WithLocale(dutchLocale).Build())
                .WithUniqueId(ContactId)
                .Build();
        }
    }
}
