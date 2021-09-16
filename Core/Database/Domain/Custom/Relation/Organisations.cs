// <copyright file="Organisation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Person type.</summary>

namespace Allors.Domain
{
    using System;
    using Meta;

    public partial class Organisations
    {
        private static readonly Guid ToggleRestrictionId = new Guid("68BB6EC4-CF15-47D1-8F87-D817419C9482");

        public Restriction ToggleRestriction => new Restrictions(this.Session).FindBy(M.Restriction.UniqueId, ToggleRestrictionId);

        private UniquelyIdentifiableSticky<Organisation> cache;

        public UniquelyIdentifiableSticky<Organisation> Cache => this.cache ??= new UniquelyIdentifiableSticky<Organisation>(this.Session);

        protected override void CustomSecure(Security security)
        {
            if (this.ToggleRestriction == null)
            {
                new RestrictionBuilder(this.Session).WithUniqueId(ToggleRestrictionId).Build();
            }

            var permissions = new Permissions(this.Session);
            var deniedPermissions = new[]
            {
                permissions.Get(this.Meta.Class, this.Meta.Name, Operations.Write),
                permissions.Get(this.Meta.Class, this.Meta.Owner, Operations.Write),
                permissions.Get(this.Meta.Class, this.Meta.Employees, Operations.Write),
            };

            this.ToggleRestriction!.DeniedPermissions = deniedPermissions;
        }
    }
}
