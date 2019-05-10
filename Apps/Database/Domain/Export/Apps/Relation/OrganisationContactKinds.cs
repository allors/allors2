// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrganisationContactKinds.cs" company="Allors bvba">
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

    public partial class OrganisationContactKinds
    {
        private static readonly Guid GeneralContactId = new Guid("EEBE4D65-C452-49C9-A583-C0FFEC385E98");
        private static readonly Guid SalesContactId = new Guid("3F9B9226-508B-4ADA-91E9-1A353BB177F3");
        private static readonly Guid SupportContactId = new Guid("E59022E2-9497-47ED-9443-37EFD791FE44");
        private static readonly Guid SupplierContactId = new Guid("ECE0813F-64DD-4A5F-93AA-147887923581");

        private UniquelyIdentifiableSticky<OrganisationContactKind> cache;

        public OrganisationContactKind GeneralContact => this.Cache[GeneralContactId];

        public OrganisationContactKind SalesContact => this.Cache[SalesContactId];

        public OrganisationContactKind SupportContact => this.Cache[SupportContactId];

        public OrganisationContactKind SupplierContact => this.Cache[SupplierContactId];

        private UniquelyIdentifiableSticky<OrganisationContactKind> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<OrganisationContactKind>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            new OrganisationContactKindBuilder(this.Session).WithDescription("General contact").WithUniqueId(GeneralContactId).Build();
            new OrganisationContactKindBuilder(this.Session).WithDescription("Sales contact").WithUniqueId(SalesContactId).Build();
            new OrganisationContactKindBuilder(this.Session).WithDescription("Support contact").WithUniqueId(SupportContactId).Build();
            new OrganisationContactKindBuilder(this.Session).WithDescription("Supplier contact").WithUniqueId(SupplierContactId).Build();
        }
    }
}