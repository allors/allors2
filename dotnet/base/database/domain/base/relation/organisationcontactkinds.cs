// <copyright file="OrganisationContactKinds.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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

        private UniquelyIdentifiableSticky<OrganisationContactKind> Cache => this.cache ??= new UniquelyIdentifiableSticky<OrganisationContactKind>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(GeneralContactId, v => v.Description = "General contact");
            merge(SalesContactId, v => v.Description = "Sales contact");
            merge(SupportContactId, v => v.Description = "Support contact");
            merge(SupplierContactId, v => v.Description = "Supplier contact");
        }
    }
}
