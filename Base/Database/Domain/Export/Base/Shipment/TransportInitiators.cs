// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransportInitiators.cs" company="Allors bvba">
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

    public partial class TransportInitiators
    {
        private static readonly Guid InternalOrganisationId = new Guid("E1A6CC9E-7783-4CE6-8586-AC16975C1B2E");
        private static readonly Guid CustomerId = new Guid("D6839B12-DA68-4266-8D30-A432F443B703");

        private UniquelyIdentifiableSticky<TransportInitiator> cache;

        public TransportInitiator InternalOrganisation => this.Cache[InternalOrganisationId];

        public TransportInitiator Customer => this.Cache[CustomerId];

        private UniquelyIdentifiableSticky<TransportInitiator> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<TransportInitiator>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new TransportInitiatorBuilder(this.Session)
                .WithName("InternalOrganisation")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Interne Organisatie").WithLocale(dutchLocale).Build())
                .WithUniqueId(InternalOrganisationId)
                .WithIsActive(true)
                .Build();

            new TransportInitiatorBuilder(this.Session)
                .WithName("Customer")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Klant").WithLocale(dutchLocale).Build())
                .WithUniqueId(CustomerId)
                .WithIsActive(true)
                .Build();
        }
    }
}
